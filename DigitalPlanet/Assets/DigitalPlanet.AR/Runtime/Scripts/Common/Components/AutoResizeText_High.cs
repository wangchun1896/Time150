using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class AutoResizeText_High : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro; // 引用 TextMeshProUGUI 组件
        public RectTransform rectTransform; // 引用 RectTransform 组件

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }
        void OnEnable()
        {
            // 设置文本内容
            UpdateText(textMeshPro.text);
        }

        public void UpdateText(string newText = "")
        {
            if (string.IsNullOrEmpty(newText)) return;

            textMeshPro.text = newText;

            // 获取文本边界的高度
            float preferredHeight = textMeshPro.preferredHeight;

            // 设置 RectTransform 的高度
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);
            //Debug.Log(rectTransform.sizeDelta.y + "-------");
        }
    }
}