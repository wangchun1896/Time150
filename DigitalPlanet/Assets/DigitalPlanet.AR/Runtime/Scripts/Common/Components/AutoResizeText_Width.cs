using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class AutoResizeText_Width : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro; // ���� TextMeshProUGUI ���
        public RectTransform rectTransform; // ���� RectTransform ���
        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateText(string newText = "")
        {
            if (string.IsNullOrEmpty(newText)) return;
            textMeshPro.text = newText;

            // �����ı��Ŀ��
            float preferredWidth = textMeshPro.preferredWidth;
            rectTransform.sizeDelta = new Vector2(preferredWidth, rectTransform.sizeDelta.y);
        }
    }
}