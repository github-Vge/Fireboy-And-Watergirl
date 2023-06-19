using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("同步ID，状态同步时标识是哪个物体，\n不同物体的ID不能相同！")]
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
        //发送当前箱子的状态信息到服务端
        MessageHandler.SendBoxState(synchronousID, transform.position, rb.velocity, transform.rotation.eulerAngles);
    }

    /// <summary>
    /// 收到箱子的状态消息时调用
    /// </summary>
    /// <param name="boxSynchronousID">同步ID</param>
    private void OnUpdateBoxState(int boxSynchronousID, Vector3 position, Vector3 velocity, Vector3 rotation)
    {
        if (synchronousID == boxSynchronousID)//是当前箱子
        {
            this.transform.position = position;
            this.rb.velocity = velocity;
            this.transform.eulerAngles = rotation;
            MessageHandler.boxPreviousVelocity = rb.velocity;
        }
    }



}
