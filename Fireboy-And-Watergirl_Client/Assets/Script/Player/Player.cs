using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    [Header("是否为男孩")]
    public bool isBoy;
    [Header("移动速度")]
    public float moveSpeed;
    [Header("跳跃速度")]
    public float jumpSpeed;

    /// <summary>当前物体的Rigidbody2D组件</summary>
    private Rigidbody2D rb;
    /// <summary>玩家当前的状态</summary>
    private PlayerState playerState;
    /// <summary>头部的动画</summary>
    private Animator headAnimator;
    /// <summary>身体的动画</summary>
    private Animator bodyAnimator;

    private void Awake()
    {
        //获取组件
        rb = GetComponent<Rigidbody2D>();
        headAnimator = transform.GetChild(0).GetComponent<Animator>();
        bodyAnimator = transform.GetChild(1).GetComponent<Animator>();
        //初始化移动和跳跃速度
        moveSpeed = moveSpeed < 0.001f ? 6f : moveSpeed;
        jumpSpeed = jumpSpeed < 0.001f ? 11f : jumpSpeed;
        //初始化玩家状态
        playerState = PlayerState.Idle;
    }


    private void Update()
    {
        PlayerMove();
        PlayerAnimation();
    }

    private void PlayerMove()
    {
        Vector2 inputVelocity;
        //获取当前Rigidbody2D的速度
        inputVelocity = rb.velocity;
        //左右移动
        float horizontal = Input.GetAxis("Horizontal");
        inputVelocity.x = horizontal * moveSpeed;
        //设置玩家转向，向左转或向右转
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Run:
                //跳跃
                float jump = Input.GetAxis("Jump");
                if (jump > 0.001f)//如果Player不在跳跃状态并且按下了跳跃（空格）键
                {
                    playerState = PlayerState.Jump;
                    inputVelocity.y = rb.velocity.y + jump * jumpSpeed;
                }
                else if (playerState == PlayerState.Idle && Mathf.Abs(inputVelocity.x) > 0.001f)
                {
                    playerState = PlayerState.Run;
                }
                else if (playerState == PlayerState.Run && Mathf.Abs(inputVelocity.x) < 0.001f)
                {
                    playerState = PlayerState.Idle;
                }

                break;
            case PlayerState.Jump:
                if (rb.velocity.y < -0.01f)//向下掉落
                {
                    playerState = PlayerState.Fall;
                }
                break;
            case PlayerState.Fall:
                if (Mathf.Abs(rb.velocity.y) < 0.001f)
                {
                    playerState = PlayerState.Idle;
                }
                break;

            default:
                break;
        }

        //设置Rigidbody2D的速度
        rb.velocity = inputVelocity;
        //同步消息
        MessageHandler.SendPlayerState(this.transform.position, rb.velocity, this.transform.localScale.x);

    }

    private void PlayerAnimation()
    {
        headAnimator.SetBool("Idle", playerState == PlayerState.Idle);
        headAnimator.SetBool("Run", playerState == PlayerState.Run);
        headAnimator.SetBool("Jump", playerState == PlayerState.Jump);
        headAnimator.SetBool("Fall", playerState == PlayerState.Fall);
        bodyAnimator.SetBool("Idle", playerState == PlayerState.Idle);
        bodyAnimator.SetBool("Run", playerState == PlayerState.Run);
        bodyAnimator.SetBool("Jump", playerState == PlayerState.Jump);
        bodyAnimator.SetBool("Fall", playerState == PlayerState.Fall);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBoy == true)
        {
            if (collision.name == "Bluewater" || collision.name == "Greenwater")
            {
                Destroy(this.gameObject);
                Debug.Log("我死了");
                MessageHandler.SendPlayerDieMessage();
            }
        }
        else if(isBoy == false)
        {
            if (collision.name == "Redwater" || collision.name == "Greenwater")
            {
                Destroy(this.gameObject);
                Debug.Log("我死了");
                MessageHandler.SendPlayerDieMessage();
            }
        }
    }



    public enum PlayerState
    {
        None,
        Idle, Run, Jump, Fall,
    }




}
