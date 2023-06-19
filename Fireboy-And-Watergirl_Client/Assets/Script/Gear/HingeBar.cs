using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeBar : MonoBehaviour
{
    private HingeJoint2D hingeJoint2D;
    [Header("对应的移动平台")]
    public MobilePlatform mobilePlatform;

    /// <summary>当前机关是否在开始状态</summary>
    private bool isOpen;
    /// <summary>机关的中间角度</summary>
    private float middleAngle;

    private void Awake()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();

        middleAngle = (hingeJoint2D.limits.max + hingeJoint2D.limits.min) / 2f;
        isOpen = false;
    }

    private void Update()
    {
        if (isOpen == true && hingeJoint2D.jointAngle > middleAngle)//开关被关闭
        {
            isOpen = false;
            mobilePlatform.Close();
        }
        else if (isOpen == false && hingeJoint2D.jointAngle < middleAngle)//开关被打开
        {
            isOpen = true;
            mobilePlatform.Open();
        }
    }


}
