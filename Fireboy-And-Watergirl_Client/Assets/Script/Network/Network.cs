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
    /// <summary>�ͻ���Socket</summary>
    private static Socket socket;
    /// <summary>������Ϣ������</summary>
    private static MemoryStream receiveStream = new MemoryStream(8 * 1024);
    /// <summary>���յ���Ϣ����Ҫ����ճ������</summary>
    private static string receiveString = string.Empty;
    /// <summary>���յ���Ϣ����</summary>
    public static Queue<NetMessage> netMessageQueue = new Queue<NetMessage>();

    /// <summary>
    /// �������ӷ����
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <returns>���ؿͻ��˵�EndPoint</returns>
    public static bool Connect(string ip, int port)
    {
        //��ʼ��Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            //���ӷ����
            socket.Connect(iPEndPoint);
            //��ʼ������Ϣ���첽���գ�
            socket.BeginReceive(receiveStream.GetBuffer(), 0, receiveStream.GetBuffer().Length, SocketFlags.None, ReceiveCallback, socket);

            return true;
        }
        catch(SocketException)
        {
            Debug.Log("�޷����ӵ������");
            return false;
        }
        
        return false;
    }

    /// <summary>
    /// �����ܵ���Ϣʱ����
    /// </summary>
    /// <param name="ar"></param>
    private static void ReceiveCallback(IAsyncResult ar)
    {
        //��ȡSocket
        Socket socket = (Socket)ar.AsyncState;
        try
        {
            //��ȡ��Ϣ����
            int receiveCount = socket.EndReceive(ar);
            if (receiveCount <= 0)
            {
                Debug.Log("��������ӶϿ�");
                //�ر�����
                socket.Close();
            }
            //��ȡ��Ϣ����
            string str = Encoding.UTF8.GetString(receiveStream.GetBuffer(), 0, receiveCount);
            //�ϲ���Ϣ
            receiveString += str;
            //���������������Ϣ��һ��������
            if (receiveString.Contains("$"))//���������������
            {
                //�ݴ�δ������������ϢƬ��
                string remainStr = receiveString.Substring(receiveString.LastIndexOf('$') + 1);
                //ȥ��Ƭ��
                receiveString = receiveString.Substring(0, receiveString.LastIndexOf('$'));
                //���÷ָ��ַ�
                char splitChar = '$';
                //�ְ�
                string[] splitedStrings = receiveString.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                foreach (string splitedString in splitedStrings)
                {
                    Debug.Log("���յ�����˵���Ϣ��" + splitedString);
                    //�����л�
                    NetMessage messages = JsonConvert.DeserializeObject<NetMessage>(splitedString);
                    lock(netMessageQueue)
                    {
                        //���뵽��Ϣ����
                        netMessageQueue.Enqueue(messages);
                    }
                }
                //����Ƭ��
                receiveString = remainStr.Length > 0 ? remainStr : string.Empty;
                //��ջ�����
                receiveStream.SetLength(0);

                //����������Ϣ
                socket.BeginReceive(receiveStream.GetBuffer(), 0, receiveStream.GetBuffer().Length, SocketFlags.None, ReceiveCallback, socket);
            }
        }
        catch (SocketException)
        {
            Debug.Log("��������ӶϿ�");
            //�ر�����
            socket.Close();
        }
    }

    /// <summary>
    /// �����˷�����Ϣ
    /// </summary>
    /// <param name="netMessage"></param>
    public static void SendMessage(NetMessage netMessage)
    {
        //��Ϣ������
        MemoryStream sendStream = new MemoryStream();
        //���л���Ϣ
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        string message = JsonConvert.SerializeObject(netMessage, settings);
        //Ϊ��Ϣ��� ��������
        message += "$";
        byte[] send = Encoding.UTF8.GetBytes(message);
        sendStream.Write(send, 0, send.Length);
        try
        {
            //������Ϣ
            socket.BeginSend(sendStream.GetBuffer(), 0, (int)sendStream.Length, SocketFlags.None, null, null);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("������Ϣʧ�ܣ�����" + ex.ToString());
        }

    }

    /// <summary>
    /// �ر������˵�����
    /// </summary>
    public static void CloesSocket()
    {
        //�ر�����
        socket.Close();
    }
    /// <summary>
    /// ��ȡ��ǰ�ͻ��˵�ID
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

