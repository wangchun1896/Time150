using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeStoryContentAllCtl : AutoSizeBase
{
    public string diraction;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void CalculateAndSetHeight()
    {
        // ��ʼ���ܸ߶�
        float maxHeight = 0f;


        // ��������һ��������
        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float height = rectTransform.rect.height;
                if (height > maxHeight)
                {
                    maxHeight = height;
                }
            }
        }
        // ���õ�ǰ����ĸ߶�
        RectTransform rectTransform_self = GetComponent<RectTransform>();
        if (rectTransform_self != null)
        {
            // �����Լ��ĸ߶ȣ��ɸ�����Ҫ����ê����Ի����ȷ��Ч��
            rectTransform_self.sizeDelta = new Vector2(rectTransform_self.sizeDelta.x, maxHeight+500);
            //onHeightChange?.Invoke();
        }
    }
}
