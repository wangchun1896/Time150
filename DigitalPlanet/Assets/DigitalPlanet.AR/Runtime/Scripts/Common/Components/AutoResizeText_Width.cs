using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class AutoResizeText_Width : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro; // 引用 TextMeshProUGUI 组件
        public RectTransform rectTransform; // 引用 RectTransform 组件
        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateText(string newText = "")
        {
            if (string.IsNullOrEmpty(newText)) return;
            textMeshPro.text = newText;

            // 计算文本的宽度
            float preferredWidth = textMeshPro.preferredWidth;
            rectTransform.sizeDelta = new Vector2(preferredWidth, rectTransform.sizeDelta.y);
        }
    }
}