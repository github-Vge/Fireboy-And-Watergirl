using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    /// <summary>单例</summary>
    public static RoomUI Instance;
    [Header("房间面板")]
    public GameObject roomPanel;
    [Header("三个子面板：房间列表，创建房间按钮，房间中")]
    public GameObject roomListPanel;
    public Button createRoomButton;
    public GameObject InRoomPanel;
    [Header("房间控件预制体")]
    public GameObject roomItemPerfab;


    private void Awake()
    {
        //初始化单例
        Instance = this;
    }

    private void Start()
    {
        //显示和隐藏相应面板
        roomListPanel.SetActive(true);
        createRoomButton.enabled = true;
        InRoomPanel.SetActive(false);
        //添加监听事件
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
    }

    public void UpdateRoomList(List<int> roomList)
    {
        Transform content = roomListPanel.transform.GetChild(0).GetChild(0);
        //先摧毁已有的控件
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        //新建新的控件
        foreach (int roomID in roomList)
        {
            GameObject roomItem = Instantiate(roomItemPerfab, content);
            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "房间：" + roomID;
            roomItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { OnJoinRoomButtonClick(roomID); });
        }
        //强制刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

    }
    /// <summary>
    /// 当接收到服务端的创建房间成功的消息时调用
    /// </summary>
    /// <param name="roomID">房间ID</param>
    public void OnCreateRoomSuccessMessage(int roomID)
    {
        roomListPanel.SetActive(false);
        createRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "退出房间";
        InRoomPanel.SetActive(true);
        InRoomPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "已在房间：" + roomID.ToString();
        createRoomButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.AddListener(OnLeaveRoomButtonClick);
        createRoomButton.enabled = true;
    }
    /// <summary>
    /// 点击“创建房间”按钮后执行的函数
    /// </summary>
    private void OnCreateRoomButtonClick()
    {
        createRoomButton.enabled = false;
        MessageHandler.SendCreateRoomMessage();
    }
    /// <summary>
    /// 点击“离开房间”按钮后执行的函数
    /// </summary>
    private void OnLeaveRoomButtonClick()
    {
        InRoomPanel.SetActive(false);
        createRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "创建房间";
        roomListPanel.SetActive(true);
        createRoomButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
        MessageHandler.SendLeaveRoomMessage();
    }
    /// <summary>
    /// 点击任间一个“加入房间”按钮后执行的函数
    /// </summary>
    /// <param name="roomID">加入的房间ID</param>
    public void OnJoinRoomButtonClick(int roomID)
    {
        MessageHandler.SendJoinRoomMessage(roomID);
    }




}
