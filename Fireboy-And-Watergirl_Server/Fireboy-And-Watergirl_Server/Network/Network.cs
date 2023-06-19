using Multiverse;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;

namespace Fireboy_And_Watergirl_Server
{
    public static class Network
    {
        /// <summary>服务端的监听Socket</summary>
        private static Socket listenSocket;
        /// <summary>所有客户端的连接信息</summary>
        private static Dictionary<Socket, ClientState> clientSocketDict = new Dictionary<Socket, ClientState>();

        /// <summary>
        /// 开始接收客户端的连接
        /// </summary>
        /// <param name="port">端口号</param>
        public static void Start(int port)
        {
            //Unity Online Services配置，与本项目无关
            MultiverseSdk sdk = new MultiverseSdk();
            sdk.ConnectAsync();

            //初始化监听Socket
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定端口
            IPAddress iPAddress = IPAddress.Any;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            //绑定端口
            listenSocket.Bind(iPEndPoint);
            //开始监听，设置最多有10个客户端连接请求（已连接的不算）
            listenSocket.Listen(10);
            //输出提示
            Console.WriteLine("[服务器]启动成功");

            //Unity Online Services配置，与本项目无关
            sdk.ReadyAsync();

            //开始接收客户端的连接和客户端的消息
            while (true)
            {
                //Select的检查列表
                List<Socket> selectList = new List<Socket>();
                //重置Select列表
                ResetSelectList(selectList);
                //获取是否有消息
                Socket.Select(selectList, null, null, 1000);
                //处理消息
                foreach (Socket socket in selectList)
                {
                    if (socket == listenSocket)
                    {
                        //接收客户端的连接
                        AcceptSocket();
                    }
                    else
                    {
                        //接收已连接客户端的消息
                        ReceiveMessage(socket);
                    }
                }
                //超时检查
                CheckPingPong();

            }


        }
        /// <summary>
        /// 重置Select列表
        /// </summary>
        /// <param name="selectList">Select列表</param>
        private static void ResetSelectList(List<Socket> selectList)
        {
            selectList.Clear();
            selectList.Add(listenSocket);
            foreach (ClientState s in clientSocketDict.Values)
            {
                selectList.Add(s.socket);
            }
        }

        /// <summary>
        /// 接收新的客户端连接
        /// </summary>
        private static void AcceptSocket()
        {
            try
            {
                //获取客户端Socket
                Socket clientSocket = listenSocket.Accept();
                Console.WriteLine("接收到客户端[" + clientSocket.RemoteEndPoint.ToString() + "]的连接");
                //新建ClientState对象
                ClientState state = new ClientState();
                //初始化ClientState数据
                state.clientID = clientSocket.RemoteEndPoint.ToString();
                state.socket = clientSocket;
                state.receiveStream = new MemoryStream(8 * 1024);
                state.message = string.Empty;
                state.lastPingTime = GetTimeStamp();
                //添加ClientState到字典中
                clientSocketDict.Add(clientSocket, state);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("无法接收到客户端的连接:" + ex.ToString());
            }
        }

        /// <summary>
        /// 接收客户端的消息
        /// </summary>
        private static void ReceiveMessage(Socket clientSocket)
        {
            //获得客户端的连接信息
            ClientState state = clientSocketDict[clientSocket];
            try
            {
                //接收消息，并获得消息长度
                int receiveCount = clientSocket.Receive(state.receiveStream.GetBuffer(), 0, state.receiveStream.GetBuffer().Length, 0);
                if (receiveCount <= 0)
                {
                    //关闭连接
                    CloseClientSocket(state);
                }
                //获取消息内容
                string str = Encoding.UTF8.GetString(state.receiveStream.GetBuffer(), 0, receiveCount);
                //合并消息
                state.message += str;
                //如果存在完整的消息（一个或多个）
                if (state.message.Contains("$"))//如果包含结束符号
                {
                    //暂存未接收完整的消息片段
                    string remainStr = state.message.Substring(state.message.LastIndexOf('$') + 1);
                    //去除片段
                    state.message = state.message.Substring(0, state.message.LastIndexOf('$'));
                    //设置分割字符
                    char splitChar = '$';
                    //分包
                    string[] splitedStrings = state.message.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string splitedString in splitedStrings)
                    {
                        Console.WriteLine("接收到第" + state.clientID + "个客户端的消息：" + splitedString);

                        //反序列化
                        NetMessage messages = JsonConvert.DeserializeObject<NetMessage>(splitedString);
                        //派遣消息
                        MessageHandler.Dispatch(state, messages);

                    }
                    //保留片段
                    state.message = remainStr.Length > 0 ? remainStr : string.Empty;
                    //清空缓冲区
                    state.receiveStream.SetLength(0);

                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                //关闭连接
                CloseClientSocket(state);
            }

        }
        /// <summary>
        /// 关闭一个客户端的连接
        /// </summary>
        /// <param name="clientSocket"></param>
        private static void CloseClientSocket(ClientState clientState)
        {
            //移除玩家
            PlayerManager.RemovePlayer(clientState.clientID);
            //从字典中移除Socket
            clientSocketDict.Remove(clientState.socket);
            //关闭Socket
            clientState.socket.Close();
            //输出提示
            Console.WriteLine("客户端["+clientState.clientID+"]失去了接连");
        }


        /// <summary>
        /// 向指定客户端发送消息
        /// </summary>
        /// <param name="clientState"></param>
        /// <param name="netMessage"></param>
        public static void SendMessage(ClientState clientState, NetMessage netMessage)
        {
            //消息缓冲区
            MemoryStream sendStream = new MemoryStream();
            //序列化消息（忽略空值）
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string message = JsonConvert.SerializeObject(netMessage, settings);
            //为消息添加 结束符号
            message += "$";
            //字符串转成byte[]
            byte[] send = Encoding.UTF8.GetBytes(message);
            //将消息写入缓冲区
            sendStream.Write(send, 0, send.Length);
            try
            {
                //发送消息
                clientState.socket.BeginSend(sendStream.GetBuffer(), 0, (int)sendStream.Length, SocketFlags.None, null, null);
                //Console.WriteLine("发送消息：" + message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("发送消息失败：" + ex.ToString());
            }

        }

        /// <summary>
        /// 向所有客户端广播消息
        /// </summary>
        /// <param name="netMessage">消息</param>
        /// <param name="negativeClient">指定哪个客户端不需要广播</param>
        public static void Broadcast(NetMessage netMessage, ClientState negativeClient = null)
        {
            List<ClientState> clientStates;
            if (negativeClient != null)//移除不需要广播的客户端
            {
                clientStates = clientSocketDict.Values.Where(t => t.clientID != negativeClient.clientID).ToList();
            }
            else
            {
                clientStates = clientSocketDict.Values.ToList();
            }
            //广播消息
            foreach (ClientState clientState in clientStates)
            {
                SendMessage(clientState, netMessage);
            }

        }

        /// <summary>
        /// 向指定房间的所有客户端广播消息
        /// </summary>
        /// <param name="roomID">房间ID</param>
        /// <param name="netMessage">消息</param>
        /// <param name="negativeClient">指定哪个客户端不需要广播</param>
        public static void Broadcast(int roomID, NetMessage netMessage, ClientState negativeClient = null)
        {
            //获取指定房间的所有客户端
            List<ClientState> clientStates = clientSocketDict.Values.Where(t => PlayerManager.GetPlayer(t.clientID).roomID == roomID).ToList();
            //移除不需要广播的客户端
            if (negativeClient != null)
            {
                clientStates = clientStates.Where(t => t.clientID != negativeClient.clientID).ToList();
            }
            //广播消息
            foreach (ClientState clientState in clientStates)
            {
                SendMessage(clientState, netMessage);
            }

        }


        //public static ClientState GetClientState(string clientID)
        //{
        //    return clientSocketDict.Values.FirstOrDefault(t => t.clientID == clientID);
        //}

        /// <summary>累积的循环次数</summary>
        private static int loopCount = 0;
        private static void CheckPingPong()
        {
            loopCount++;
            if (loopCount == 100)//每经过100次循环检测一下Ping消息（心跳消息）
            {
                loopCount = 0;
                //现在的时间戳
                long timeNow = GetTimeStamp();
                //检测客户端是否很久没有Ping
                foreach (ClientState clientState in clientSocketDict.Values)
                {
                    if (timeNow - clientState.lastPingTime > 10 * 3)//30秒没有收到Ping消息（心跳消息）
                    {
                        Console.WriteLine("客户端无Ping消息，关闭Socket：" + clientState.socket.RemoteEndPoint.ToString());
                        CloseClientSocket(clientState);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }


    }

    /// <summary>
    /// 客户端的连接信息
    /// </summary>
    public class ClientState
    {
        /// <summary>客户端的唯一ID，其实就是EndPoint</summary>
        public string clientID;
        /// <summary>客户端的连接Socket</summary>
        public Socket socket;
        /// <summary>接受消息缓冲区</summary>
        public MemoryStream receiveStream;
        /// <summary>已有的消息，主要用于粘包处理</summary>
        public string message;
        /// <summary>上一次Ping消息的时间</summary>
        public long lastPingTime = 0;
    }
}
