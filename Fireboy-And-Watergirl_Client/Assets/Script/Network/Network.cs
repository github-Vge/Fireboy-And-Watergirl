using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class Network
{
    /// <summary>客户端Socket</summary>
    private static Socket socket;
    /// <summary>接收消息缓冲区</summary>
    private static MemoryStream receiveStream = new MemoryStream(8 * 1024);
    /// <summary>接收的消息，主要用于粘包处理</summary>
    private static string receiveString = string.Empty;
    /// <summary>接收的消息队列</summary>
    public static Queue<NetMessage> netMessageQueue = new Queue<NetMessage>();

    /// <summary>
    /// 尝试连接服务端
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <returns>返回客户端的EndPoint</returns>
    public static bool Connect(string ip, int port)
    {
        //初始化Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            //连接服务端
            socket.Connect(iPEndPoint);
            //开始接收消息（异步接收）
            socket.BeginReceive(receiveStream.GetBuffer(), 0, receiveStream.GetBuffer().Length, SocketFlags.None, ReceiveCallback, socket);

            return true;
        }
        catch(SocketException)
        {
            Debug.Log("无法连接到服务端");
            return false;
        }
        
        return false;
    }

    /// <summary>
    /// 当接受到消息时调用
    /// </summary>
    /// <param name="ar"></param>
    private static void ReceiveCallback(IAsyncResult ar)
    {
        //获取Socket
        Socket socket = (Socket)ar.AsyncState;
        try
        {
            //获取消息长度
            int receiveCount = socket.EndReceive(ar);
            if (receiveCount <= 0)
            {
                Debug.Log("服务端连接断开");
                //关闭连接
                socket.Close();
            }
            //获取消息内容
            string str = Encoding.UTF8.GetString(receiveStream.GetBuffer(), 0, receiveCount);
            //合并消息
            receiveString += str;
            //如果存在完整的消息（一个或多个）
            if (receiveString.Contains("$"))//如果包含结束符号
            {
                //暂存未接收完整的消息片段
                string remainStr = receiveString.Substring(receiveString.LastIndexOf('$') + 1);
                //去除片段
                receiveString = receiveString.Substring(0, receiveString.LastIndexOf('$'));
                //设置分割字符
                char splitChar = '$';
                //分包
                string[] splitedStrings = receiveString.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                foreach (string splitedString in splitedStrings)
                {
                    Debug.Log("接收到服务端的消息：" + splitedString);
                    //反序列化
                    NetMessage messages = JsonConvert.DeserializeObject<NetMessage>(splitedString);
                    lock(netMessageQueue)
                    {
                        //加入到消息队列
                        netMessageQueue.Enqueue(messages);
                    }
                }
                //保留片段
                receiveString = remainStr.Length > 0 ? remainStr : string.Empty;
                //清空缓冲区
                receiveStream.SetLength(0);

                //继续接收消息
                socket.BeginReceive(receiveStream.GetBuffer(), 0, receiveStream.GetBuffer().Length, SocketFlags.None, ReceiveCallback, socket);
            }
        }
        catch (SocketException)
        {
            Debug.Log("服务端连接断开");
            //关闭连接
            socket.Close();
        }
    }

    /// <summary>
    /// 向服务端发送消息
    /// </summary>
    /// <param name="netMessage"></param>
    public static void SendMessage(NetMessage netMessage)
    {
        //消息缓冲区
        MemoryStream sendStream = new MemoryStream();
        //序列化消息
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        string message = JsonConvert.SerializeObject(netMessage, settings);
        //为消息添加 结束符号
        message += "$";
        byte[] send = Encoding.UTF8.GetBytes(message);
        sendStream.Write(send, 0, send.Length);
        try
        {
            //发送消息
            socket.BeginSend(sendStream.GetBuffer(), 0, (int)sendStream.Length, SocketFlags.None, null, null);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("发送消息失败，错误：" + ex.ToString());
        }

    }

    /// <summary>
    /// 关闭与服务端的连接
    /// </summary>
    public static void CloesSocket()
    {
        //关闭连接
        socket.Close();
    }
    /// <summary>
    /// 获取当前客户端的ID
    /// </summary>
    /// <returns></returns>
    public static string GetClientID()
    {
        if (socket.Connected)
        {
            return socket.LocalEndPoint.ToString();
        }
        else
        {
            return null;
        }
    }




}

