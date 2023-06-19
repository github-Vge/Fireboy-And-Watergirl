using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    /// <summary>����</summary>
    public static EndGameUI Instance;
    [Header("������Ϸ���")]
    public GameObject endGamePanel;
    [Header("���¿�ʼ��ť")]
    public Button restartGameButton;
    [Header("�ȴ���ʾ�ı�")]
    public TextMeshProUGUI waitingTipText;


    private void Awake()
    {
        Instance = this;
        //�󶨰�ť�¼�
        restartGameButton.onClick.AddListener(OnRestartGameButtonClick);
    }


    private void OnEnable()
    {
        MessageHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        MessageHandler.GameOverEvent -= OnGameOverEvent;
    }



    public void ShowPanel(bool bShow)
    {
        endGamePanel.SetActive(bShow);
        restartGameButton.gameObject.SetActive(true);
        waitingTipText.gameObject.SetActive(false);
    }


    /// <summary>
    /// �յ�����˵���Ϸ������Ϣʱ����
    /// </summary>
    /// <param name="deadPlayerID"></param>
    private void OnGameOverEvent()
    {
        ShowPanel(true);
    }

    /// <summary>
    /// �����¿�ʼ��ť�����ʱ����
    /// </summary>
    private void OnRestartGameButtonClick()
    {
        restartGameButton.gameObject.SetActive(false);
        waitingTipText.gameObject.SetActive(true);
        MessageHandler.SendRestartGameMessage();
    }




}
