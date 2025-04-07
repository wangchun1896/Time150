using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class ScrollRectHandler : MonoBehaviour
    {
        public ScrollRect scrollRect;

        private bool canTrigger = true; // 控制触发的标志
        public float threshold = 0.1f; // 上拉的阈值，用于避免小的滑动被错误识别

        private void Start()
        {
            scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        private void OnScrollRectValueChanged(Vector2 scrollPosition)
        {
            // 获取 ScrollRect 的内容和视口的高度
            float contentHeight = scrollRect.content.rect.height;
            float viewportHeight = scrollRect.viewport.rect.height;

            // 判断滚动是否到了底部
            if (scrollPosition.y <= 0 && scrollRect.vertical)
            {
                // 检查用户是否正在向上推拉
                if (scrollPosition.y < -threshold && canTrigger)
                {
                    // 触发事件
                    Debug.Log("User is pulling up at the bottom!");

                    // 设置标志位为 false，防止重复触发
                    canTrigger = false;
                    StartCoroutine(AsyOpenCanTrigger());
                }
            }
            else
            {
                // 如果用户没有在底部，重置标志位
                //  canTrigger = true;
            }
        }

        private IEnumerator AsyOpenCanTrigger()
        {
            yield return new WaitForSeconds(1);
            canTrigger = true;
        }
    }
}