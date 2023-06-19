using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>����</summary>
    public static GameManager Instance;
    [Header("������ɫ��Ԥ����")]
    public GameObject fireboyPerfab;
    public GameObject watergirlPerfab;

    /// <summary>��ǰ�ͻ����Ƿ�Ϊ�к�</summary>
    public bool isBoy;

    private GameObject fireboy;
    private GameObject watergirl;

    private void Awake()
    {
        //��ʼ������
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
        //���س���
        Scene scene = SceneManager.GetSceneByName("Level1");
        if (!scene.isLoaded)//�������û�б����أ�����س���
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName("MainUI");
        if (!scene.isLoaded)//�������û�б����أ�����س���
            SceneManager.LoadScene("MainUI", LoadSceneMode.Additive);


    }
    /// <summary>
    /// �յ���ʼ��Ϸ����Ϣ�������������
    /// </summary>
    /// <param name="isBoy">��ǰ�ͻ����Ƿ�Ϊ�к�</param>
    public void InitPlayer(bool isBoy)
    {
        //��¼ֵ
        this.isBoy = isBoy;
        //���û�г���������س���
        Scene scene = SceneManager.GetSceneByName("Level1");
        if (!scene.isLoaded)//�������û�б����أ�����س���
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        //���ط������
        RoomUI.Instance.roomPanel.SetActive(false);
        //���ؽ������
        EndGameUI.Instance.endGamePanel.SetActive(false);
        //����һ֡�����������
        StartCoroutine(LoadPlayer(isBoy));
    }
    /// <summary>
    /// ����ĺ���InitPlayer�е���
    /// </summary>
    /// <param name="isBoy"></param>
    /// <returns></returns>
    private IEnumerator LoadPlayer(bool isBoy)
    {
        yield return 1;

        //�������������
        Transform startPoint = GameObject.Find("StartPoint").transform;
        Vector3 startPoint_Fireboy = startPoint.GetChild(0).position;
        Vector3 startPoint_Watergirl = startPoint.GetChild(1).position;
        //ʵ����
        fireboy = Instantiate(fireboyPerfab, startPoint_Fireboy, Quaternion.identity, this.transform);
        fireboy.name = "Fireboy";
        watergirl = Instantiate(watergirlPerfab, startPoint_Watergirl, Quaternion.identity, this.transform);
        watergirl.name = "Watergirl";
        //��ʼ����ɫ
        if (isBoy)//���к�
        {
            fireboy.AddComponent<Player>().isBoy = true;
            watergirl.AddComponent<OtherPlayer>().isBoy = false;
        }
        else//��Ů��
        {
            watergirl.AddComponent<Player>().isBoy = false;
            fireboy.AddComponent<OtherPlayer>().isBoy = true;
        }
    }

    /// <summary>
    /// �յ�����˵���Ϸ������Ϣʱ����
    /// </summary>
    private void OnGameOverEvent()
    {
        Destroy(fireboy);
        Destroy(watergirl);
        SceneManager.UnloadSceneAsync("Level1");
    }


}
