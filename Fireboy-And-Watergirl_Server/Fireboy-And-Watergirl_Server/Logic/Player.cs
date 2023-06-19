
namespace Fireboy_And_Watergirl_Server
{
    public class Player
    {
        //玩家id
        public string id = "";
        //指向ClientState
        public ClientState clientState;
        //构造函数
        public Player(ClientState state)
        {
            this.id = state.clientID;
            this.clientState = state;
            this.state = PlayerState.Prepare;
        }

        //boy还是girl
        public bool isBoy;
        //坐标
        public float positionX;
        public float positionY;
        public float positionZ;
        //速度
        public float velocityX;
        public float velocityY;
        public float velocityZ;
        //缩放
        public float scaleX;
        public float scaleY;
        public float scaleZ;

        //在哪个房间
        public int roomID = 0;

       
        public enum PlayerState
        {
            None,
            Play, End, Prepare,
        }
        /// <summary>玩家当前状态</summary>
        public PlayerState state;

        /// <summary>
        /// 给当前玩家所在的客户端发送信息
        /// </summary>
        /// <param name="netMessage">消息</param>
        public void Send(NetMessage netMessage)
        {
            Network.SendMessage(clientState, netMessage);
        }
    }
}
