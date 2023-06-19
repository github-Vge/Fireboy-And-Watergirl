using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    /// <summary>����</summary>
    public static RoomUI Instance;
    [Header("�������")]
    public GameObject roomPanel;
    [Header("��������壺�����б��������䰴ť��������")]
    public GameObject roomListPanel;
    public Button createRoomButton;
    public GameObject InRoomPanel;
    [Header("����ؼ�Ԥ����")]
    public GameObject roomItemPerfab;


    private void Awake()
    {
        //��ʼ������
        Instance = this;
    }

    private void Start()
    {
        //��ʾ��������Ӧ���
        roomListPanel.SetActive(true);
        createRoomButton.enabled = true;
        InRoomPanel.SetActive(false);
        //��Ӽ����¼�
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
    }

    public void UpdateRoomList(List<int> roomList)
    {
        Transform content = roomListPanel.transform.GetChild(0).GetChild(0);
        //�ȴݻ����еĿؼ�
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        //�½��µĿؼ�
        foreach (int roomID in roomList)
        {
            GameObject roomItem = Instantiate(roomItemPerfab, content);
            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���䣺" + roomID;
            roomItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { OnJoinRoomButtonClick(roomID); });
        }
        //ǿ��ˢ�²���
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

    }
    /// <summary>
    /// �����յ�����˵Ĵ�������ɹ�����Ϣʱ����
    /// </summary>
    /// <param name="roomID">����ID</param>
    public void OnCreateRoomSuccessMessage(int roomID)
    {
        roomListPanel.SetActive(false);
        createRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "�˳�����";
        InRoomPanel.SetActive(true);
        InRoomPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���ڷ��䣺" + roomID.ToString();
        createRoomButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.AddListener(OnLeaveRoomButtonClick);
        createRoomButton.enabled = true;
    }
    /// <summary>
    /// ������������䡱��ť��ִ�еĺ���
    /// </summary>
    private void OnCreateRoomButtonClick()
    {
        createRoomButton.enabled = false;
        MessageHandler.SendCreateRoomMessage();
    }
    /// <summary>
    /// ������뿪���䡱��ť��ִ�еĺ���
    /// </summary>
    private void OnLeaveRoomButtonClick()
    {
        InRoomPanel.SetActive(false);
        createRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "��������";
        roomListPanel.SetActive(true);
        createRoomButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
        MessageHandler.SendLeaveRoomMessage();
    }
    /// <summary>
    /// ����μ�һ�������뷿�䡱��ť��ִ�еĺ���
    /// </summary>
    /// <param name="roomID">����ķ���ID</param>
    public void OnJoinRoomButtonClick(int roomID)
    {
        MessageHandler.SendJoinRoomMessage(roomID);
    }




}
