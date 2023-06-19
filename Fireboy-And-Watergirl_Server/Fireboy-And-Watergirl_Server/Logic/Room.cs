
namespace Fireboy_And_Watergirl_Server
{
    public class Room
    {
        //房间id
        public int roomID = 0;
        //最大玩家数
        public const int maxPlayer = 2;
        //玩家列表
        public List<Player> playerList = new List<Player>();
        //房主id
        public string ownerID;
        //状态
        public enum RoomState
        {
            Perpare = 0,
            Start = 1,
            End = 2,

        }
        /// <summary>当前房间的状态</summary>
        public RoomState state = RoomState.Perpare;
        /// <summary>已进门的玩家数</summary>
        private int enterDoorCount = 0;

        /// <summary>
        /// 向当前房间添加玩家
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        public void AddPlayer(string playerID)
        {
            //获取玩家
            Player player = PlayerManager.GetPlayer(playerID);
            //判断是否有该玩家
            if (player == null)
            {
                Console.WriteLine("room.AddPlayer fail, 没有该玩家");
                return;
            }
            //判断房间人数
            if (playerList.Count >= maxPlayer)
            {
                Console.WriteLine("room.AddPlayer fail, 房间人数已满");
                return;
            }
            //判断玩家是否已经在房间里
            if (playerList.Contains(player))
            {
                Console.WriteLine("room.AddPlayer fail, 玩家已经在房间里了");
                return;
            }
            //加入玩家列表
            playerList.Add(player);
            //设置玩家数据
            player.isBoy = playerList.Count != maxPlayer;
            player.roomID = roomID;
            //设置房主
            if (string.IsNullOrEmpty(ownerID))
            {
                ownerID = player.id;
            }
            //玩家够了，开始游戏
            if (playerList.Count == maxPlayer)
            {
                MessageHandler.SendStartGameMessage(roomID);
                state = RoomState.Start;
                playerList.ForEach(player => player.state = Player.PlayerState.Play);
            }

        }

        /// <summary>
        /// 将指定玩家移除当前房间
        /// </summary>
        /// <param name="playerID"></param>
        public void RemovePlayer(string playerID)
        {
            Player player = PlayerManager.GetPlayer(playerID);
            player.roomID = 0;
            playerList.Remove(player);
        }
        /// <summary>
        /// 玩家死亡
        /// </summary>
        /// <param name="playerID"></param>
        public void PlayerDie()
        {
            playerList.ForEach(player => player.state = Player.PlayerState.End);
            MessageHandler.SendGameOverMessage(roomID);
        }
        /// <summary>
        /// 玩家进入/离开通关门洞
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        /// <param name="isEnter">是否为进入</param>
        public void PlayerEnterDoor(string playerID, bool isEnter)
        {
            if(isEnter)//进门
            {
                enterDoorCount++;
                if (enterDoorCount == maxPlayer)//所有玩家都进门了
                {
                    //通关
                    state = RoomState.End;
                    MessageHandler.SendGameWinMessage(roomID);
                }
            }
            else//出门
            {
                enterDoorCount--;
            }
        }
        /// <summary>
        /// 玩家做好重新开始游戏的准备时调用
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        public void PlayerRestartGame(string playerID)
        {
            Player player = PlayerManager.GetPlayer(playerID);
            player.state = Player.PlayerState.Prepare;
            int perparedPlayerCount = playerList.FindAll(t => t.state == Player.PlayerState.Prepare).Count;
            if (perparedPlayerCount == maxPlayer)//所有玩家都准备了
            {
                //开始游戏
                state = RoomState.Start;
                MessageHandler.SendStartGameMessage(roomID);
                playerList.ForEach(t => t.state = Player.PlayerState.Play);
            }
        }


    }
}
