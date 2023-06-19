using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    /// <summary>单例</summary>
    public static GameWinUI Instance;
    [Header("游戏胜利面板")]
    public GameObject gameWinPanel;
    [Header("下一关按钮")]
    public Button nextLevelButton;


    private void Awake()
    {
        //初始化单例
        Instance = this;
        //绑定按钮事件
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
    /// 显示或隐藏游戏胜利面板
    /// </summary>
    /// <param name="bShow"></param>
    public void ShowGameWinPanel(bool bShow)
    {
        gameWinPanel.SetActive(bShow);
    }
    /// <summary>
    /// 收到游戏胜利的消息后调用
    /// </summary>
    private void OnGameWinEvent()
    {
        ShowGameWinPanel(true);
    }
    /// <summary>
    /// 点击“下一关”按钮后执行的函数
    /// </summary>
    private void OnNextLevelButtonClick()
    {
        gameWinPanel.SetActive(false);
        MessageHandler.SendRestartGameMessage();

    }


}
