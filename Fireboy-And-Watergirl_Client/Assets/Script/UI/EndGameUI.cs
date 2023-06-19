using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    /// <summary>单例</summary>
    public static EndGameUI Instance;
    [Header("结束游戏面板")]
    public GameObject endGamePanel;
    [Header("重新开始按钮")]
    public Button restartGameButton;
    [Header("等待提示文本")]
    public TextMeshProUGUI waitingTipText;


    private void Awake()
    {
        Instance = this;
        //绑定按钮事件
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
    /// 收到服务端的游戏结束消息时调用
    /// </summary>
    /// <param name="deadPlayerID"></param>
    private void OnGameOverEvent()
    {
        ShowPanel(true);
    }

    /// <summary>
    /// 当重新开始按钮被点击时调用
    /// </summary>
    private void OnRestartGameButtonClick()
    {
        restartGameButton.gameObject.SetActive(false);
        waitingTipText.gameObject.SetActive(true);
        MessageHandler.SendRestartGameMessage();
    }




}
