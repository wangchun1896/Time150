using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TimeStar.DigitalPlant
{
    public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public ScrollRect scrollRect; // 将你的 ScrollRect 拖拽到这里
        public float snapSpeed = 10f; // 对齐速度
        private bool isDragging = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true; // 标记为正在拖动
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false; // 标记为停止拖动
            StartCoroutine(SnapToNearestItem()); // 开始对齐协程
        }

        private System.Collections.IEnumerator SnapToNearestItem()
        {
            // 获取当前的 content 偏移量
            float contentPosX = scrollRect.content.anchoredPosition.x;

            // 计算 item 的大小（假设所有 item 的宽度相同）
            float itemWidth = scrollRect.content.GetChild(0).GetComponent<RectTransform>().rect.width;
            int itemCount = scrollRect.content.childCount;

            // 计算最近的 item 索引
            int nearestItemIndex = Mathf.RoundToInt(-contentPosX / itemWidth);
            nearestItemIndex = Mathf.Clamp(nearestItemIndex, 0, itemCount - 1); // 确保索引在有效范围内

            // 计算目标位置
            float targetPosX = -nearestItemIndex * itemWidth;

            // 使用协程平滑移动到目标位置
            while (!Mathf.Approximately(scrollRect.content.anchoredPosition.x, targetPosX))
            {
                float newPositionX = Mathf.Lerp(scrollRect.content.anchoredPosition.x, targetPosX, Time.deltaTime * snapSpeed);
                scrollRect.content.anchoredPosition = new Vector2(newPositionX, scrollRect.content.anchoredPosition.y);
                yield return null; // 等待下一帧
            }

            // 确保最后精确设置到目标位置
            scrollRect.content.anchoredPosition = new Vector2(targetPosX, scrollRect.content.anchoredPosition.y);
        }
    }
}