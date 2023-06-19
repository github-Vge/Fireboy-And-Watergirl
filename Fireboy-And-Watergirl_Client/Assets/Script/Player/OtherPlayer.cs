using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class OtherPlayer : MonoBehaviour
{
    [Header("是否为男孩")]
    public bool isBoy;

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
        //初始化玩家状态
        playerState = PlayerState.Idle;
    }

    private void OnEnable()
    {
        MessageHandler.UpdatePlayerState += OnUpdatePlayerState;
    }

    private void OnDisable()
    {
        MessageHandler.UpdatePlayerState -= OnUpdatePlayerState;
    }

    private void Update()
    {
        PlayerMove();
        PlayerAnimation();
    }

    private void PlayerMove()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Run:
                //跳跃
                if (rb.velocity.y > 0.001f)//如果Player速度向上
                {
                    playerState = PlayerState.Jump;
                }
                else if (playerState == PlayerState.Idle && Mathf.Abs(rb.velocity.x) > 0.001f)
                {
                    playerState = PlayerState.Run;
                }
                else if (playerState == PlayerState.Run && Mathf.Abs(rb.velocity.x) < 0.001f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Jump:
                if (rb.velocity.y < -0.001f)//向下掉落
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

    /// <summary>
    /// 收到状态消息
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="velocity">速度</param>
    /// <param name="scaleX">X轴缩放</param>
    private void OnUpdatePlayerState(Vector3 position, Vector3 velocity, float scaleX)
    {
        this.transform.position = position;
        rb.velocity = velocity;
        this.transform.localScale = new Vector3(scaleX, 1, 1);
    }




}
