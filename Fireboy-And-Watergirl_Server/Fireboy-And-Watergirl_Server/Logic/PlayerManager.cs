

namespace Fireboy_And_Watergirl_Server
{
    public static class PlayerManager
    {
        /// <summary>玩家列表，key:玩家ID，value:玩家信息</summary>
        private static Dictionary<string, Player> playerDict = new Dictionary<string, Player>();
        /// <summary>外部类可以访问的属性</summary>
        public static Dictionary<string, Player> PlayerDict { get => playerDict; }


        /// <summary>
        /// 根据玩家ID获取玩家实例
        /// </summary>
        /// <param name="id">玩家ID</param>
        /// <returns></returns>
        public static Player GetPlayer(string id)
        {
            if (playerDict.ContainsKey(id))
            {
                return playerDict[id];
            }
            throw new NullReferenceException();
        }
        /// <summary>
        /// 向全局玩家字典中添加玩家
        /// </summary>
        /// <param name="id">玩家ID</param>
        /// <param name="player">玩家实例</param>
        public static void AddPlayer(string id, Player player)
        {
            playerDict.Add(id, player);
        }
        /// <summary>
        /// 从玩家字典中删除对应玩家ID的玩家
        /// </summary>
        /// <param name="id"></param>
        public static void RemovePlayer(string id)
        {
            if (!playerDict.ContainsKey(id)) return;

            //将玩家从房间中移出
            if (GetPlayer(id).roomID != 0)
            {
                RoomManager.PlayerLeave(id);
            }
            //将玩家从字典中移出
            playerDict.Remove(id);
        }

    }
}
