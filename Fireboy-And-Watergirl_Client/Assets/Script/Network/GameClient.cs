using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClient : MonoBehaviour
{
    /// <summary>����</summary>
    public static GameClient Instance;
    [Header("IP��ַ")]
    public string ip;
    [Header("�˿ں�")]
    public int port;
    /// <summary>�Ƿ����ӵ��˷����</summary>
    private bool bConnect = false;
    /// <summary>��¼��һ�η���Ping��Ϣ��ʱ��</summary>
    private float lastPingTime;
    /// <summary>��һ�ν��յ�Pong��Ϣ��ʲôʱ��</summary>
    public float lastPongTime;

    private void Awake()
    {
        //��ʼ������
        Instance = this;
        //��ʼ���ϴ�Ping��ʱ��
        lastPingTime = Time.time;
        lastPongTime = Time.time;
    }

    private void Start()
    {
        //���ӵ�������
        ip = ip == string.Empty ? "127.0.0.1" : ip;
        port = port == 0 ? 8888 : port;
        if (Network.Connect(ip, port))//���ӳɹ�
        {
            //���͵�¼��Ϣ
            MessageHandler.SendLoginMessage();

            bConnect = true;
            //����Ping��Ϣ
            lastPingTime = Time.time;
            MessageHandler.SendPingPongMessage();
        }
        else//����ʧ��
        {

        }


    }

    private void Update()
    {
        if(bConnect)//������ӵ��������
        {
            lock (Network.netMessageQueue)
            {
                while(Network.netMessageQueue.Count > 0)
                {
                    //��Ϣ����
                    NetMessage netMessage = Network.netMessageQueue.Dequeue();
                    //��ǲ��Ϣ
                    MessageHandler.Dispatch(netMessage);
                }
            }
            //����������Ϣ��ÿ��10���ӷ���һ�Σ�
            if (Time.time - lastPingTime > 10f)
            {
                lastPingTime = Time.time;
                MessageHandler.SendPingPongMessage();
                //���Pong��Ϣ���û�յ��ˣ��ɷ���˷��͹����ģ�
                if ((Time.time - lastPongTime) > 30f)//������30��
                {
                    //�Ͽ������˵�����
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
