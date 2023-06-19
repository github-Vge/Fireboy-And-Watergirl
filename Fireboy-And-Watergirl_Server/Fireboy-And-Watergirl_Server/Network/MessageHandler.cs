
namespace Fireboy_And_Watergirl_Server
{
    public static class MessageHandler
    {
        /// <summary>
        /// 消息派遣
        /// </summary>
        /// <param name="netMessage"></param>
        public static void Dispatch(ClientState clientState, NetMessage netMessage)
        {
            if (netMessage.pingPong != null) { MessagePingPong(clientState, netMessage.pingPong); }
            else if (netMessage.login != null) { MessageLogin(clientState, netMessage.login); }
            else if (netMessage.createRoom != null) { MessageCreateRoom(clientState, netMessage.createRoom); }
            else if (netMessage.joinRoom != null) { MessageJoinRoom(clientState, netMessage.joinRoom); }
            else if (netMessage.leaveRoom != null) { MessageLeaveRoom(clientState, netMessage.leaveRoom); }
            else if (netMessage.playerState != null) { MessagePlayerState(clientState, netMessage.playerState); }
            else if (netMessage.boxState != null) { MessageBoxState(clientState, netMessage.boxState); }
            else if (netMessage.playerDie != null) { MessagePlayerDie(clientState, netMessage.playerDie); }
            else if (netMessage.playerEnterDoor != null) { MessagePlayerEnterDoor(clientState, netMessage.playerEnterDoor); }
            else if (netMessage.restartGame != null) { MessageRestartGame(clientState, netMessage.restartGame); }

        }

        #region 接收消息

        /// <summary>
        /// 心跳消息
        /// </summary>
        private static void MessagePingPong(ClientState clientState, NetMessage_PingPong message)
        {
            //更新时间戳
            clientState.lastPingTime = Network.GetTimeStamp();
            //直接回消息
            NetMessage netMessage = new NetMessage();
            netMessage.pingPong = message;
            Network.SendMessage(clientState, netMessage);
        }

        private static void MessageLogin(ClientState clientState, NetMessage_Login message)
        {
            //新增玩家
            Player player = new Player(clientState);
            //添加玩家到全局字典中
            PlayerManager.AddPlayer(player.id, player);
            //更新时间戳
            clientState.lastPingTime = Network.GetTimeStamp();
            //回调消息（登录成功）
            SendLoginMessage(clientState);
            //发送房间列表消息
            SendRoomListMessage();
        }

        private static void MessageCreateRoom(ClientState clientState, NetMessage_CreateRoom message)
        {
            //创建房间
            Room room = RoomManager.AddRoom();
            //向房间中添加玩家
            room.AddPlayer(clientState.clientID);
            //发送创建房间成功消息
            message.roomID = room.roomID;
            NetMessage netMessage = new NetMessage();
            netMessage.createRoom = message;
            PlayerManager.GetPlayer(clientState.clientID).Send(netMessage);
            //发送房间列表消息
            SendRoomListMessage();
        }

        private static void MessageJoinRoom(ClientState clientState, NetMessage_JoinRoom message)
        {
            //获取房间
            Room room = RoomManager.GetRoom(message.roomID);
            //向房间中添加玩家
            room.AddPlayer(clientState.clientID);
            //更新房间列表消息
            SendRoomListMessage();
        }

        private static void MessageLeaveRoom(ClientState clientState, NetMessage_LeaveRoom message)
        {
            //玩家离开当前房间
            RoomManager.PlayerLeave(clientState.clientID);
        }

        private static void MessagePlayerState(ClientState clientState, NetMessage_PlayerState message)
        {
            //获取玩家所在的房间
            int roomID = PlayerManager.GetPlayer(clientState.clientID).roomID;
            NetMessage netMessage = new NetMessage();
            netMessage.playerState = message;
            //向其他玩家同步状态信息
            Network.Broadcast(roomID, netMessage, clientState);
        }

        private static void MessageBoxState(ClientState clientState, NetMessage_BoxState message)
        {
            //获取玩家所在的房间
            int roomID = PlayerManager.GetPlayer(clientState.clientID).roomID;
            NetMessage netMessage = new NetMessage();
            netMessage.boxState = message;
            //向所有玩家同步状态信息
            Network.Broadcast(roomID, netMessage, clientState);
        }


        private static void MessagePlayerDie(ClientState clientState, NetMessage_PlayerDie message)
        {
            Player player = PlayerManager.GetPlayer(clientState.clientID);
            Room room = RoomManager.GetRoom(player.roomID);
            room.PlayerDie();
        }

        private static void MessagePlayerEnterDoor(ClientState clientState, NetMessage_PlayerEnterDoor message)
        {
            Room room = RoomManager.roomDict.Values.First(t => t.roomID == PlayerManager.GetPlayer(clientState.clientID).roomID);
            room.PlayerEnterDoor(clientState.clientID, message.isEnter);
        }

        private static void MessageRestartGame(ClientState clientState, NetMessage_RestartGame message)
        {
            RoomManager.PlayerRestartGame(clientState.clientID);
        }

        #endregion

        #region 发送消息

        public static void SendLoginMessage(ClientState clientState)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.login = new NetMessage_Login();
            Network.SendMessage(clientState, netMessage);
        }

        public static void SendRoomListMessage()
        {
            NetMessage netMessage = new NetMessage();
            netMessage.roomList = new NetMessage_RoomList();
            //返回所有房间人数为1的房间（人数为2则房间满了）
            netMessage.roomList.roomIDList = RoomManager.roomDict.Keys.Where(t => RoomManager.roomDict[t].playerList.Count == 1).ToList();
            Network.Broadcast(netMessage);
        }

        public static void SendStartGameMessage(int roomID)
        {
            foreach (Player player in RoomManager.GetRoom(roomID).playerList)
            {
                NetMessage netMessage = new NetMessage();
                netMessage.startGame = new NetMessage_StartGame();
                netMessage.startGame.isBoy = player.isBoy;
                player.Send(netMessage);
            }
        }

        public static void SendGameWinMessage(int roomID)
        {
            foreach (Player player in RoomManager.GetRoom(roomID).playerList)
            {
                NetMessage netMessage = new NetMessage();
                netMessage.gameWin = new NetMessage_GameWin();
                netMessage.gameWin.roomID = roomID;
                player.Send(netMessage);
            }
        }

        public static void SendGameOverMessage(int roomID)
        {
            foreach (Player player in RoomManager.GetRoom(roomID).playerList)
            {
                NetMessage netMessage = new NetMessage();
                netMessage.gameOver = new NetMessage_GameOver();
                netMessage.gameOver.roomID = roomID;
                player.Send(netMessage);
            }
        }

        #endregion



    }
}
