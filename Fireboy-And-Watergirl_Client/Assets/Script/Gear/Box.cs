using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("ͬ��ID��״̬ͬ��ʱ��ʶ���ĸ����壬\n��ͬ�����ID������ͬ��")]
    public int synchronousID;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        MessageHandler.UpdateBoxStateEvent += OnUpdateBoxState;
    }

    private void OnDisable()
    {
        MessageHandler.UpdateBoxStateEvent -= OnUpdateBoxState;
    }



    private void Update()
    {
        //���͵�ǰ���ӵ�״̬��Ϣ�������
        MessageHandler.SendBoxState(synchronousID, transform.position, rb.velocity, transform.rotation.eulerAngles);
    }

    /// <summary>
    /// �յ����ӵ�״̬��Ϣʱ����
    /// </summary>
    /// <param name="boxSynchronousID">ͬ��ID</param>
    private void OnUpdateBoxState(int boxSynchronousID, Vector3 position, Vector3 velocity, Vector3 rotation)
    {
        if (synchronousID == boxSynchronousID)//�ǵ�ǰ����
        {
            this.transform.position = position;
            this.rb.velocity = velocity;
            this.transform.eulerAngles = rotation;
            MessageHandler.boxPreviousVelocity = rb.velocity;
        }
    }



}
