using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearButton : MonoBehaviour
{

    [Header("对应的移动平台")]
    public MobilePlatform mobilePlatform;

    /// <summary>渲染组件</summary>
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
