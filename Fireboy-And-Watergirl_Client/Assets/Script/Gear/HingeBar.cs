using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeBar : MonoBehaviour
{
    private HingeJoint2D hingeJoint2D;
    [Header("��Ӧ���ƶ�ƽ̨")]
    public MobilePlatform mobilePlatform;

    /// <summary>��ǰ�����Ƿ��ڿ�ʼ״̬</summary>
    private bool isOpen;
    /// <summary>���ص��м�Ƕ�</summary>
    private float middleAngle;

    private void Awake()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();

        middleAngle = (hingeJoint2D.limits.max + hingeJoint2D.limits.min) / 2f;
        isOpen = false;
    }

    private void Update()
    {
        if (isOpen == true && hingeJoint2D.jointAngle > middleAngle)//���ر��ر�
        {
            isOpen = false;
            mobilePlatform.Close();
        }
        else if (isOpen == false && hingeJoint2D.jointAngle < middleAngle)//���ر���
        {
            isOpen = true;
            mobilePlatform.Open();
        }
    }


}
