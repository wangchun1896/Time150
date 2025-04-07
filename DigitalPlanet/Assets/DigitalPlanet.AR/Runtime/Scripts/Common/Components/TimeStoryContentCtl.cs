using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class TimeStoryContentCtl : AutoSizeBase
    {
        public string diraction;
        private TimeStoryContentAllCtl timeStoryContentAllCtl;
        protected override void Awake()
        {
            base.Awake();
            timeStoryContentAllCtl = transform.parent.GetComponent<TimeStoryContentAllCtl>();
            onHeightChange.AddListener(timeStoryContentAllCtl.CalculateAndSetHeight);
        }
        private void OnDestroy()
        {
            if (onHeightChange != null)
                onHeightChange.RemoveListener(timeStoryContentAllCtl.CalculateAndSetHeight);
        }

        public override void CalculateAndSetHeight()
        {
            // 初始化总高度
            float totalHeight = 0f;
            float space = GetComponent<VerticalLayoutGroup>().spacing;
            space = (space != 0) ? (transform.childCount - 1) * space : 0;

            // 遍历所有一级子物体
            foreach (Transform child in transform)
            {
                // 计算子物体的高度
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                if (childRectTransform != null)
                {
                    totalHeight += childRectTransform.rect.height; // 获取高度并累加
                }
            }
            totalHeight += space;
            // 设置当前物体的高度
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 设置自己的高度，可根据需要调整锚点和以获得正确的效果
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
                onHeightChange?.Invoke();
            }
        }
    }
}