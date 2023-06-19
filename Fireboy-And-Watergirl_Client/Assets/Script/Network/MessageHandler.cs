using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageHandler
{
    /// <summary>
    /// 消息派遣
    /// </summary>
    /// <param name="netMessage"></param>
    public static void Dispatch(NetMessage netMessage)
    {
        if (netMessage.pingPong != null) { MessagePingPong(netMessage.pingPong); }
        else if (netMessage.login != null) { MessageLogin(netMessage.login); }
        else if (netMessage.createRoom != null) { MessageCreateRoomSuccess(netMessage.createRoom); }
        else if (netMessage.roomList != null) { MessageRoomList(netMessage.roomList); }
        else if (netMessage.startGame != null) { MessageStartGame(netMessage.startGame); }
        else if (netMessage.playerState != null) { MessagePlayerState(netMessage.playerState); }
        else if (netMessage.boxState != null) { MessageBoxState(netMessage.boxState); }
        else if (netMessage.gameOver != null) { MessageGameOver(netMessage.gameOver); }
        else if (netMessage.gameWin != null) { MessageGameWin(netMessage.gameWin); }
    }

    #region 接收消息

    private static void MessagePingPong(NetMessage_PingPong message)
    {
        GameClient.Instance.lastPongTime = Time.time;
    }

    /// <summary>
    /// 接收到登录成功的消息
    /// </summary>
    /// <param name="login"></param>
    private static void MessageLogin(NetMessage_Login message)
    {
        Debug.Log("登录成功");
        //显示房间列表
        RoomUI.Instance.roomPanel.SetActive(true);
    }


    private static void MessageRoomList(NetMessage_RoomList meesage)
    {
        RoomUI.Instance.UpdateRoomList(meesage.roomIDList);
    }

    private static void MessageCreateRoomSuccess(NetMessage_CreateRoom message)
    {
        RoomUI.Instance.OnCreateRoomSuccessMessage(message.roomID);
    }

    /// <summary>
    /// 接收到开始游戏的消息
    /// </summary>
    /// <param name="message"></param>
    private static void MessageStartGame(NetMessage_StartGame message)
    {
        Debug.Log("游戏开始");
        GameManager.Instance.InitPlayer(message.isBoy);
    }

    public static event Action<Vector3, Vector3, float> UpdatePlayerState;
    private static void MessagePlayerState(NetMessage_PlayerState message)
    {
        Vector3 position = new Vector3(message.positionX, message.positionY, message.positionZ);
        Vector3 velocity = new Vector3(message.velocityX, message.velocityY, message.velocityZ);
        float scaleX = message.scaleX;
        UpdatePlayerState?.Invoke(position, velocity, scaleX);
    }

    public static event Action<int, Vector3, Vector3, Vector3> UpdateBoxStateEvent;
    private static void MessageBoxState(NetMessage_BoxState message)
    {
        Vector3 position = new Vector3(message.positionX, message.positionY, message.positionZ);
        Vector3 velocity = new Vector3(message.velocityX, message.velocityY, message.velocityZ);
        Vector3 rotation = new Vector3(message.rotationX, message.rotationY, message.rotationZ);
        UpdateBoxStateEvent?.Invoke(message.boxID, position, velocity, rotation);
    }

    public static event Action GameOverEvent;
    private static void MessageGameOver(NetMessage_GameOver message)
    {
        GameOverEvent?.Invoke();
    }

    public static event Action GameWinEvent;
    private static void MessageGameWin(NetMessage_GameWin message)
    {
        GameWinEvent?.Invoke();
    }
    #endregion

    #region 发送消息

    public static void SendPingPongMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.pingPong = new NetMessage_PingPong();
        Network.SendMessage(netMessage);
    }

    public static void SendLoginMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.login = new NetMessage_Login();
        Network.SendMessage(netMessage);
    }

    public static void SendCreateRoomMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.createRoom = new NetMessage_CreateRoom();
        Network.SendMessage(netMessage);
    }

    public static void SendJoinRoomMessage(int roomID)
    {
        NetMessage netMessage = new NetMessage();
        netMessage.joinRoom = new NetMessage_JoinRoom();
        netMessage.joinRoom.roomID = roomID;
        Network.SendMessage(netMessage);
    }

    public static void SendLeaveRoomMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.leaveRoom = new NetMessage_LeaveRoom();
        Network.SendMessage(netMessage);
    }

    private static float tikTime = 0f;
    private static Vector3 previousVelocity = Vector3.zero;
    /// <summary>
    /// 发送状态消息，应该每帧同步
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="scaleX"></param>
    public static void SendPlayerState(Vector3 position, Vector3 velocity, float scaleX)
    {
        //最快同步30帧
        tikTime += Time.deltaTime;
        if (tikTime < 0.033f)
            return;
        else
            tikTime = 0f;
        //速度不变就不同步
        if ((velocity - previousVelocity).sqrMagnitude < 0.01f)
            return;
        else
            previousVelocity = velocity;

        NetMessage netMessage = new NetMessage();
        netMessage.playerState = new NetMessage_PlayerState();
        netMessage.playerState.positionX = position.x;
        netMessage.playerState.positionY = position.y;
        netMessage.playerState.positionZ = position.z;
        netMessage.playerState.velocityX = velocity.x;
        netMessage.playerState.velocityY = velocity.y;
        netMessage.playerState.velocityZ = velocity.z;
        netMessage.playerState.scaleX = scaleX;
        Network.SendMessage(netMessage);
    }

    private static float boxTikTime = 0f;
    public static Vector3 boxPreviousVelocity = Vector3.zero;
    /// <summary>
    /// 发送状态消息，应该每帧同步
    /// </summary>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="scaleX"></param>
    public static void SendBoxState(int boxIntanceID, Vector3 position, Vector3 velocity, Vector3 rotation)
    {
        //最快同步30帧
        boxTikTime += Time.deltaTime;
        if (boxTikTime < 0.033f)
            return;
        else
            boxTikTime = 0f;
        //速度不变就不同步
        if ((velocity - boxPreviousVelocity).sqrMagnitude < 0.01f)
            return;
        else
            boxPreviousVelocity = velocity;

        NetMessage netMessage = new NetMessage();
        netMessage.boxState = new NetMessage_BoxState();
        netMessage.boxState.boxID = boxIntanceID;
        netMessage.boxState.positionX = position.x;
        netMessage.boxState.positionY = position.y;
        netMessage.boxState.positionZ = position.z;
        netMessage.boxState.velocityX = velocity.x;
        netMessage.boxState.velocityY = velocity.y;
        netMessage.boxState.velocityZ = velocity.z;
        netMessage.boxState.rotationX = rotation.x;
        netMessage.boxState.rotationY = rotation.y;
        netMessage.boxState.rotationZ = rotation.z;

        Network.SendMessage(netMessage);
    }

    public static void SendPlayerDieMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.playerDie = new NetMessage_PlayerDie();
        Network.SendMessage(netMessage);
    }

    public static void SendPlayerEnterDoorMessage(bool isEnter)
    {
        NetMessage netMessage = new NetMessage();
        netMessage.playerEnterDoor = new NetMessage_PlayerEnterDoor();
        netMessage.playerEnterDoor.isEnter = isEnter;
        Network.SendMessage(netMessage);
    }

    public static void SendRestartGameMessage()
    {
        NetMessage netMessage = new NetMessage();
        netMessage.restartGame= new NetMessage_RestartGame();
        Network.SendMessage(netMessage);
    }

    #endregion



}
