


namespace Fireboy_And_Watergirl_Server
{
    /// <summary>
    /// 传输消息协议，哪个对象不为空，则处理哪个消息
    /// </summary>
    public class NetMessage
    {
        public NetMessage_PingPong pingPong;
        public NetMessage_Login login;
        public NetMessage_CreateRoom createRoom;
        public NetMessage_JoinRoom joinRoom;
        public NetMessage_LeaveRoom leaveRoom;
        public NetMessage_RoomList roomList;
        public NetMessage_StartGame startGame;
        public NetMessage_PlayerState playerState;
        public NetMessage_BoxState boxState;
        public NetMessage_PlayerDie playerDie;
        public NetMessage_PlayerEnterDoor playerEnterDoor;
        public NetMessage_GameOver gameOver;
        public NetMessage_GameWin gameWin;
        public NetMessage_RestartGame restartGame;

    }

    /// <summary>
    /// 心跳消息，用于检测客户端是否还在连接中
    /// </summary>
    public class NetMessage_PingPong
    {
        
    }

    /// <summary>
    /// 登录消息
    /// 客户端->服务端：玩家登入
    /// 服务端->客户端：登录成功
    /// </summary>
    public class NetMessage_Login
    {

    }

    /// <summary>
    /// 创建房间消息
    /// 客户端->服务端：创建一个房间
    /// 服务端->客户端：创建房间成功，并返回房间ID
    /// </summary>
    public class NetMessage_CreateRoom
    {
        public int roomID;
    }

    /// <summary>
    /// 加入房间消息
    /// 客户端->服务端：加入一个房间，并指定一个房间ID
    /// </summary>
    public class NetMessage_JoinRoom
    {
        public int roomID;
    }

    /// <summary>
    /// 离开房间消息
    /// 客户端->服务端：离开当前的房间
    /// </summary>
    public class NetMessage_LeaveRoom
    {

    }

    /// <summary>
    /// 房间列表消息
    /// 服务端->客户端：发送当前所有的房间列表（在房间列表改变时发送）
    /// </summary>
    public class NetMessage_RoomList
    {
        public List<int> roomIDList;
    }

    /// <summary>
    /// 开始游戏消息
    /// 服务端->客户端：通知指定房间的所有玩家开始游戏
    /// </summary>
    public class NetMessage_StartGame
    {
        public int roomID;
        /// <summary>是否是男孩，当前角色只有两个：火焰男孩、寒冰女孩</summary>
        public bool isBoy;
    }

    /// <summary>
    /// 玩家的状态信息
    /// 客户端->服务端：发送当前客户端玩家的状态信息
    /// 服务端->客户端：将一个客户端玩家的状态信息转发给房间的另一名玩家
    /// </summary>
    public class NetMessage_PlayerState
    {
        public float positionX;
        public float positionY;
        public float positionZ;
        public float velocityX;
        public float velocityY;
        public float velocityZ;
        public float scaleX;
    }
    /// <summary>
    /// 箱子的状态信息
    /// 客户端->服务端：发送当前客户端箱子的状态信息
    /// 服务端->客户端：将一个客户端箱子的状态信息转发给房间的另一名玩家
    /// </summary>
    public class NetMessage_BoxState
    {
        /// <summary>箱子的唯一ID，由客户端手动指定</summary>
        public int boxID;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float velocityX;
        public float velocityY;
        public float velocityZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
    }
    /// <summary>
    /// 玩家死亡的消息
    /// 客户端->服务端：通知服务端，当前客户端的玩家死亡
    /// </summary>
    public class NetMessage_PlayerDie
    {

    }
    /// <summary>
    /// 进入通关门洞的消息
    /// 客户端->服务端：通知服务端，当前客户端的玩家进入了通关门洞
    /// </summary>
    public class NetMessage_PlayerEnterDoor
    {
        public bool isEnter;
    }
    /// <summary>
    /// 游戏结束（失败）的消息
    /// 服务端->客户端：通知指定房间的所有玩家游戏结束（失败）
    /// </summary>
    public class NetMessage_GameOver
    {
        public int roomID;
    }
    /// <summary>
    /// 游戏胜利的消息
    /// 服务端->客户端：通知指定房间的所有玩家游戏胜利
    /// </summary>
    public class NetMessage_GameWin
    {
        public int roomID;
    }
    /// <summary>
    /// 重新开始游戏的消息
    /// 客户端->服务端：已做好重新开始游戏的准备
    /// 服务端->客户端：通知指定房间的所有玩家重新开始游戏
    /// </summary>
    public class NetMessage_RestartGame
    {
        public int roomID;
    }



}
