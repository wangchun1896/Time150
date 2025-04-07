using UnityEngine;
using TMPro;

namespace TimeStar.DigitalPlant
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutoSizeTextMeshProUGUI : AutoSizeBase
    {
        public float maxWidth = 290f; // �����
        public float oneLineH = 40f;
        public float twoLineH = 70f;
        public TextMeshProUGUI textMeshPro;

        public void SetText(string text)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (IsTextExceedingWidth(text, maxWidth))
            {
                // ������������ĸ߶�
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 70f);
            }
            else
            {
                // ������������ĸ߶�
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 40);
            }
            onHeightChange.Invoke();
        }

        public bool IsTextExceedingWidth(string text, float maxWidth)
        {
            // �����ı�
            textMeshPro.text = text;
            // ǿ�Ƹ��������Լ��㵱ǰ�ı��ĳߴ�
            textMeshPro.ForceMeshUpdate();
            // ��ȡ��ǰ�ı��Ŀ��
            float currentWidth = textMeshPro.preferredWidth;
            // �ж��ı�����Ƿ񳬹������
            return currentWidth > maxWidth;
        }
    }
}