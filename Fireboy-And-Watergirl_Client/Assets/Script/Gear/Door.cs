using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("是否为男孩的门")]
    public bool isBoy;

    private Animator animator;
    /// <summary>当前的门是否打开</summary>
    private bool isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBoy == true && collision.name == "Fireboy")
        {
            animator.SetBool("Open", true);
            PlayerEnterDoor(true);
        }
        else if (isBoy == false && collision.name == "Watergirl")
        {
            animator.SetBool("Open", true);
            PlayerEnterDoor(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBoy == true && collision.name == "Fireboy")
        {
            animator.SetBool("Open", false);
            PlayerEnterDoor(false);
        }
        else if (isBoy == false && collision.name == "Watergirl")
        {
            animator.SetBool("Open", false);
            PlayerEnterDoor(false);
        }

    }

    private void PlayerEnterDoor(bool isEnter)
    {
        if (GameManager.Instance.isBoy == isBoy)
        {
            MessageHandler.SendPlayerEnterDoorMessage(isEnter);
        }
    }



}
