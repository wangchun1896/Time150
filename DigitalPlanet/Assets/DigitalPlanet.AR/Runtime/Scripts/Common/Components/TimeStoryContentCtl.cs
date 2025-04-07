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
            // ��ʼ���ܸ߶�
            float totalHeight = 0f;
            float space = GetComponent<VerticalLayoutGroup>().spacing;
            space = (space != 0) ? (transform.childCount - 1) * space : 0;

            // ��������һ��������
            foreach (Transform child in transform)
            {
                // ����������ĸ߶�
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                if (childRectTransform != null)
                {
                    totalHeight += childRectTransform.rect.height; // ��ȡ�߶Ȳ��ۼ�
                }
            }
            totalHeight += space;
            // ���õ�ǰ����ĸ߶�
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // �����Լ��ĸ߶ȣ��ɸ�����Ҫ����ê����Ի����ȷ��Ч��
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
                onHeightChange?.Invoke();
            }
        }
    }
}