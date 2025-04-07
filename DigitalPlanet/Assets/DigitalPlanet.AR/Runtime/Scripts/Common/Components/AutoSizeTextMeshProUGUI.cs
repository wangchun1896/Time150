using UnityEngine;
using TMPro;

namespace TimeStar.DigitalPlant
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutoSizeTextMeshProUGUI : AutoSizeBase
    {
        public float maxWidth = 290f; // 最大宽度
        public float oneLineH = 40f;
        public float twoLineH = 70f;
        public TextMeshProUGUI textMeshPro;

        public void SetText(string text)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (IsTextExceedingWidth(text, maxWidth))
            {
                // 最终设置组件的高度
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 70f);
            }
            else
            {
                // 最终设置组件的高度
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 40);
            }
            onHeightChange.Invoke();
        }

        public bool IsTextExceedingWidth(string text, float maxWidth)
        {
            // 设置文本
            textMeshPro.text = text;
            // 强制更新网格以计算当前文本的尺寸
            textMeshPro.ForceMeshUpdate();
            // 获取当前文本的宽度
            float currentWidth = textMeshPro.preferredWidth;
            // 判断文本宽度是否超过最大宽度
            return currentWidth > maxWidth;
        }
    }
}