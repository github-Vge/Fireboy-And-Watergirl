using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClient : MonoBehaviour
{
    /// <summary>单例</summary>
    public static GameClient Instance;
    [Header("IP地址")]
    public string ip;
    [Header("端口号")]
    public int port;
    /// <summary>是否连接到了服务端</summary>
    private bool bConnect = false;
    /// <summary>记录上一次发送Ping消息的时间</summary>
    private float lastPingTime;
    /// <summary>上一次接收到Pong消息是什么时间</summary>
    public float lastPongTime;

    private void Awake()
    {
        //初始化单例
        Instance = this;
        //初始化上次Ping的时间
        lastPingTime = Time.time;
        lastPongTime = Time.time;
    }

    private void Start()
    {
        //连接到服务器
        ip = ip == string.Empty ? "127.0.0.1" : ip;
        port = port == 0 ? 8888 : port;
        if (Network.Connect(ip, port))//连接成功
        {
            //发送登录消息
            MessageHandler.SendLoginMessage();

            bConnect = true;
            //发送Ping消息
            lastPingTime = Time.time;
            MessageHandler.SendPingPongMessage();
        }
        else//连接失败
        {

        }


    }

    private void Update()
    {
        if(bConnect)//如果连接到服务端了
        {
            lock (Network.netMessageQueue)
            {
                while(Network.netMessageQueue.Count > 0)
                {
                    //消息出队
                    NetMessage netMessage = Network.netMessageQueue.Dequeue();
                    //派遣消息
                    MessageHandler.Dispatch(netMessage);
                }
            }
            //发送心跳消息（每过10秒钟发送一次）
            if (Time.time - lastPingTime > 10f)
            {
                lastPingTime = Time.time;
                MessageHandler.SendPingPongMessage();
                //检测Pong消息多久没收到了（由服务端发送过来的）
                if ((Time.time - lastPongTime) > 30f)//超过了30秒
                {
                    //断开与服务端的连接
                    Network.CloesSocket();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (bConnect)
        {
            Network.CloesSocket();
        }
    }

}
