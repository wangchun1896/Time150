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
        // 初始化总高度
        float maxHeight = 0f;


        // 遍历所有一级子物体
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
        // 设置当前物体的高度
        RectTransform rectTransform_self = GetComponent<RectTransform>();
        if (rectTransform_self != null)
        {
            // 设置自己的高度，可根据需要调整锚点和以获得正确的效果
            rectTransform_self.sizeDelta = new Vector2(rectTransform_self.sizeDelta.x, maxHeight+500);
            //onHeightChange?.Invoke();
        }
    }
}
