using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ϢЭ�飬�ĸ�����Ϊ�գ������ĸ���Ϣ
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
/// ������Ϣ�����ڼ�⵱ǰ�ͻ����Ƿ���������
/// </summary>
public class NetMessage_PingPong
{

}

/// <summary>
/// ��¼��Ϣ
/// �ͻ���->����ˣ���ҵ���
/// �����->�ͻ��ˣ���¼�ɹ�
/// </summary>
public class NetMessage_Login
{

}

/// <summary>
/// ����������Ϣ
/// �ͻ���->����ˣ�����һ������
/// �����->�ͻ��ˣ���������ɹ��������ط���ID
/// </summary>
public class NetMessage_CreateRoom
{
    public int roomID;
}

/// <summary>
/// ���뷿����Ϣ
/// �ͻ���->����ˣ�����һ�����䣬��ָ��һ������ID
/// </summary>
public class NetMessage_JoinRoom
{
    public int roomID;
}

/// <summary>
/// �뿪������Ϣ
/// �ͻ���->����ˣ��뿪��ǰ�ķ���
/// </summary>
public class NetMessage_LeaveRoom
{

}

/// <summary>
/// �����б���Ϣ
/// �����->�ͻ��ˣ����͵�ǰ���еķ����б��ڷ����б�ı�ʱ���ͣ�
/// </summary>
public class NetMessage_RoomList
{
    public List<int> roomIDList;
}

/// <summary>
/// ��ʼ��Ϸ��Ϣ
/// �����->�ͻ��ˣ�ָ֪ͨ�������������ҿ�ʼ��Ϸ
/// </summary>
public class NetMessage_StartGame
{
    public int roomID;
    /// <summary>�Ƿ����к�����ǰ��ɫֻ�������������к�������Ů��</summary>
    public bool isBoy;
}

/// <summary>
/// ��ҵ�״̬��Ϣ
/// �ͻ���->����ˣ����͵�ǰ�ͻ�����ҵ�״̬��Ϣ
/// �����->�ͻ��ˣ���һ���ͻ�����ҵ�״̬��Ϣת�����������һ�����
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
/// ���ӵ�״̬��Ϣ
/// �ͻ���->����ˣ����͵�ǰ�ͻ������ӵ�״̬��Ϣ
/// �����->�ͻ��ˣ���һ���ͻ������ӵ�״̬��Ϣת�����������һ�����
/// </summary>
public class NetMessage_BoxState
{
    /// <summary>���ӵ�ΨһID���ɿͻ����ֶ�ָ��</summary>
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
/// �����������Ϣ
/// �ͻ���->����ˣ�֪ͨ����ˣ���ǰ�ͻ��˵��������
/// </summary>
public class NetMessage_PlayerDie
{

}
/// <summary>
/// ����ͨ���Ŷ�����Ϣ
/// �ͻ���->����ˣ�֪ͨ����ˣ���ǰ�ͻ��˵���ҽ�����ͨ���Ŷ�
/// </summary>
public class NetMessage_PlayerEnterDoor
{
    public bool isEnter;
}
/// <summary>
/// ��Ϸ������ʧ�ܣ�����Ϣ
/// �����->�ͻ��ˣ�ָ֪ͨ����������������Ϸ������ʧ�ܣ�
/// </summary>
public class NetMessage_GameOver
{
    public int roomID;
}
/// <summary>
/// ��Ϸʤ������Ϣ
/// �����->�ͻ��ˣ�ָ֪ͨ����������������Ϸʤ��
/// </summary>
public class NetMessage_GameWin
{
    public int roomID;
}
/// <summary>
/// ���¿�ʼ��Ϸ����Ϣ
/// �ͻ���->����ˣ����������¿�ʼ��Ϸ��׼��
/// �����->�ͻ��ˣ�ָ֪ͨ�����������������¿�ʼ��Ϸ
/// </summary>
public class NetMessage_RestartGame
{
    public int roomID;
}