using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>单例</summary>
    public static GameManager Instance;
    [Header("两个角色的预制体")]
    public GameObject fireboyPerfab;
    public GameObject watergirlPerfab;

    /// <summary>当前客户端是否为男孩</summary>
    public bool isBoy;

    private GameObject fireboy;
    private GameObject watergirl;

    private void Awake()
    {
        //初始化单例
        Instance = this;
    }

    private void OnEnable()
    {
        MessageHandler.GameOverEvent += OnGameOverEvent;
        MessageHandler.GameWinEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        MessageHandler.GameOverEvent -= OnGameOverEvent;
        MessageHandler.GameWinEvent -= OnGameOverEvent;
    }

    private void Start()
    {
        //加载场景
        Scene scene = SceneManager.GetSceneByName("Level1");
        if (!scene.isLoaded)//如果场景没有被加载，则加载场景
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName("MainUI");
        if (!scene.isLoaded)//如果场景没有被加载，则加载场景
            SceneManager.LoadScene("MainUI", LoadSceneMode.Additive);


    }
    /// <summary>
    /// 收到开始游戏的消息后，生成两个玩家
    /// </summary>
    /// <param name="isBoy">当前客户端是否为男孩</param>
    public void InitPlayer(bool isBoy)
    {
        //记录值
        this.isBoy = isBoy;
        //如果没有场景，则加载场景
        Scene scene = SceneManager.GetSceneByName("Level1");
        if (!scene.isLoaded)//如果场景没有被加载，则加载场景
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        //隐藏房间面板
        RoomUI.Instance.roomPanel.SetActive(false);
        //隐藏结束面板
        EndGameUI.Instance.endGamePanel.SetActive(false);
        //在下一帧加载两个玩家
        StartCoroutine(LoadPlayer(isBoy));
    }
    /// <summary>
    /// 上面的函数InitPlayer中调用
    /// </summary>
    /// <param name="isBoy"></param>
    /// <returns></returns>
    private IEnumerator LoadPlayer(bool isBoy)
    {
        yield return 1;

        //获得两个出生点
        Transform startPoint = GameObject.Find("StartPoint").transform;
        Vector3 startPoint_Fireboy = startPoint.GetChild(0).position;
        Vector3 startPoint_Watergirl = startPoint.GetChild(1).position;
        //实例化
        fireboy = Instantiate(fireboyPerfab, startPoint_Fireboy, Quaternion.identity, this.transform);
        fireboy.name = "Fireboy";
        watergirl = Instantiate(watergirlPerfab, startPoint_Watergirl, Quaternion.identity, this.transform);
        watergirl.name = "Watergirl";
        //初始化角色
        if (isBoy)//是男孩
        {
            fireboy.AddComponent<Player>().isBoy = true;
            watergirl.AddComponent<OtherPlayer>().isBoy = false;
        }
        else//是女孩
        {
            watergirl.AddComponent<Player>().isBoy = false;
            fireboy.AddComponent<OtherPlayer>().isBoy = true;
        }
    }

    /// <summary>
    /// 收到服务端的游戏结束消息时调用
    /// </summary>
    private void OnGameOverEvent()
    {
        Destroy(fireboy);
        Destroy(watergirl);
        SceneManager.UnloadSceneAsync("Level1");
    }


}
