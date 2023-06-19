using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class OtherPlayer : MonoBehaviour
{
    [Header("�Ƿ�Ϊ�к�")]
    public bool isBoy;

    /// <summary>��ǰ�����Rigidbody2D���</summary>
    private Rigidbody2D rb;
    /// <summary>��ҵ�ǰ��״̬</summary>
    private PlayerState playerState;
    /// <summary>ͷ���Ķ���</summary>
    private Animator headAnimator;
    /// <summary>����Ķ���</summary>
    private Animator bodyAnimator;

    private void Awake()
    {
        //��ȡ���
        rb = GetComponent<Rigidbody2D>();
        headAnimator = transform.GetChild(0).GetComponent<Animator>();
        bodyAnimator = transform.GetChild(1).GetComponent<Animator>();
        //��ʼ�����״̬
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
                //��Ծ
                if (rb.velocity.y > 0.001f)//���Player�ٶ�����
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
                if (rb.velocity.y < -0.001f)//���µ���
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
    /// �յ�״̬��Ϣ
    /// </summary>
    /// <param name="position">λ��</param>
    /// <param name="velocity">�ٶ�</param>
    /// <param name="scaleX">X������</param>
    private void OnUpdatePlayerState(Vector3 position, Vector3 velocity, float scaleX)
    {
        this.transform.position = position;
        rb.velocity = velocity;
        this.transform.localScale = new Vector3(scaleX, 1, 1);
    }




}
