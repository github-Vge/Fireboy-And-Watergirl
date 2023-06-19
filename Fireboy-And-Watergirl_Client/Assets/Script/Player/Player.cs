using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    [Header("�Ƿ�Ϊ�к�")]
    public bool isBoy;
    [Header("�ƶ��ٶ�")]
    public float moveSpeed;
    [Header("��Ծ�ٶ�")]
    public float jumpSpeed;

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
        //��ʼ���ƶ�����Ծ�ٶ�
        moveSpeed = moveSpeed < 0.001f ? 6f : moveSpeed;
        jumpSpeed = jumpSpeed < 0.001f ? 11f : jumpSpeed;
        //��ʼ�����״̬
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
        //��ȡ��ǰRigidbody2D���ٶ�
        inputVelocity = rb.velocity;
        //�����ƶ�
        float horizontal = Input.GetAxis("Horizontal");
        inputVelocity.x = horizontal * moveSpeed;
        //�������ת������ת������ת
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
                //��Ծ
                float jump = Input.GetAxis("Jump");
                if (jump > 0.001f)//���Player������Ծ״̬���Ұ�������Ծ���ո񣩼�
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
                if (rb.velocity.y < -0.01f)//���µ���
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

        //����Rigidbody2D���ٶ�
        rb.velocity = inputVelocity;
        //ͬ����Ϣ
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
                Debug.Log("������");
                MessageHandler.SendPlayerDieMessage();
            }
        }
        else if(isBoy == false)
        {
            if (collision.name == "Redwater" || collision.name == "Greenwater")
            {
                Destroy(this.gameObject);
                Debug.Log("������");
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
