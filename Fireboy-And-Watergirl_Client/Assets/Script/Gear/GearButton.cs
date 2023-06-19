using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearButton : MonoBehaviour
{

    [Header("��Ӧ���ƶ�ƽ̨")]
    public MobilePlatform mobilePlatform;

    /// <summary>��Ⱦ���</summary>
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        mobilePlatform.openCount++;
        if (mobilePlatform.openCount > 0)
        {
            mobilePlatform.Open();
            spriteRenderer.enabled = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        mobilePlatform.openCount--;
        if (mobilePlatform.openCount == 0)
        {
            mobilePlatform.Close();
            spriteRenderer.enabled = true;
        }
    }







}
