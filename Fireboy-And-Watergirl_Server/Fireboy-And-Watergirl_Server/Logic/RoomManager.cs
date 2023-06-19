

namespace Fireboy_And_Watergirl_Server
{
    public static class RoomManager
    {
        /// <summary>房间列表</summary>
        public static Dictionary<int, Room> roomDict = new Dictionary<int, Room>();


        /// <summary>
        /// 创建房间
        /// </summary>
        /// <returns>房间对象</returns>
        public static Room AddRoom()
        {
            int newRoomID = 0;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!roomDict.ContainsKey(i))
                {
                    newRoomID = i;
                    break;
                }
            }

            Room room = new Room();
            room.roomID = newRoomID;
            roomDict.Add(room.roomID, room);
            return room;
        }
        /// <summary>
        /// 根据房间ID获取到房间对象
        /// </summary>
        /// <param name="roomID">房间ID</param>
        /// <returns>Room对象</returns>
        public static Room GetRoom(int roomID)
        {
            return roomDict.Values.First(t => t.roomID == roomID);
        }

        /// <summary>
        /// 玩家离开所在房间时调用
        /// </summary>
        /// <param name="playerID">离开玩家的ID</param>
        public static void PlayerLeave(string playerID)
        {
            Room room = roomDict.Values.First(t => t.roomID == PlayerManager.GetPlayer(playerID).roomID);
            room.RemovePlayer(playerID);
            if (room.playerList.Count == 0)
            {
                //移除房间
                roomDict.Remove(room.roomID);
            }
            MessageHandler.SendRoomListMessage();
        }

        /// <summary>
        /// 玩家做好重新开始游戏的准备时调用
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        public static void PlayerRestartGame(string playerID)
        {
            Room room = roomDict.Values.First(t => t.roomID == PlayerManager.GetPlayer(playerID).roomID);
            room.PlayerRestartGame(playerID);
        }


    }
}
