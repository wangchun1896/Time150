using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class AutoResizeText_High : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro; // ���� TextMeshProUGUI ���
        public RectTransform rectTransform; // ���� RectTransform ���

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }
        void OnEnable()
        {
            // �����ı�����
            UpdateText(textMeshPro.text);
        }

        public void UpdateText(string newText = "")
        {
            if (string.IsNullOrEmpty(newText)) return;

            textMeshPro.text = newText;

            // ��ȡ�ı��߽�ĸ߶�
            float preferredHeight = textMeshPro.preferredHeight;

            // ���� RectTransform �ĸ߶�
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);
            //Debug.Log(rectTransform.sizeDelta.y + "-------");
        }
    }
}