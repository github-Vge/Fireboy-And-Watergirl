using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    /// <summary>����</summary>
    public static GameWinUI Instance;
    [Header("��Ϸʤ�����")]
    public GameObject gameWinPanel;
    [Header("��һ�ذ�ť")]
    public Button nextLevelButton;


    private void Awake()
    {
        //��ʼ������
        Instance = this;
        //�󶨰�ť�¼�
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
    }

    private void OnEnable()
    {
        MessageHandler.GameWinEvent += OnGameWinEvent;
    }

    private void OnDisable()
    {
        MessageHandler.GameWinEvent -= OnGameWinEvent;
    }

    /// <summary>
    /// ��ʾ��������Ϸʤ�����
    /// </summary>
    /// <param name="bShow"></param>
    public void ShowGameWinPanel(bool bShow)
    {
        gameWinPanel.SetActive(bShow);
    }
    /// <summary>
    /// �յ���Ϸʤ������Ϣ�����
    /// </summary>
    private void OnGameWinEvent()
    {
        ShowGameWinPanel(true);
    }
    /// <summary>
    /// �������һ�ء���ť��ִ�еĺ���
    /// </summary>
    private void OnNextLevelButtonClick()
    {
        gameWinPanel.SetActive(false);
        MessageHandler.SendRestartGameMessage();

    }


}
