using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TimeStar.DigitalPlant
{
    public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public ScrollRect scrollRect; // ����� ScrollRect ��ק������
        public float snapSpeed = 10f; // �����ٶ�
        private bool isDragging = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true; // ���Ϊ�����϶�
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false; // ���Ϊֹͣ�϶�
            StartCoroutine(SnapToNearestItem()); // ��ʼ����Э��
        }

        private System.Collections.IEnumerator SnapToNearestItem()
        {
            // ��ȡ��ǰ�� content ƫ����
            float contentPosX = scrollRect.content.anchoredPosition.x;

            // ���� item �Ĵ�С���������� item �Ŀ����ͬ��
            float itemWidth = scrollRect.content.GetChild(0).GetComponent<RectTransform>().rect.width;
            int itemCount = scrollRect.content.childCount;

            // ��������� item ����
            int nearestItemIndex = Mathf.RoundToInt(-contentPosX / itemWidth);
            nearestItemIndex = Mathf.Clamp(nearestItemIndex, 0, itemCount - 1); // ȷ����������Ч��Χ��

            // ����Ŀ��λ��
            float targetPosX = -nearestItemIndex * itemWidth;

            // ʹ��Э��ƽ���ƶ���Ŀ��λ��
            while (!Mathf.Approximately(scrollRect.content.anchoredPosition.x, targetPosX))
            {
                float newPositionX = Mathf.Lerp(scrollRect.content.anchoredPosition.x, targetPosX, Time.deltaTime * snapSpeed);
                scrollRect.content.anchoredPosition = new Vector2(newPositionX, scrollRect.content.anchoredPosition.y);
                yield return null; // �ȴ���һ֡
            }

            // ȷ�����ȷ���õ�Ŀ��λ��
            scrollRect.content.anchoredPosition = new Vector2(targetPosX, scrollRect.content.anchoredPosition.y);
        }
    }
}