using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class ButtonController : MonoBehaviour
    {
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f; // Ŀ��Ŵ���
        public Transform targetTr;
        public float scaleSpeed = 2f; // �Ŵ�ͻ�ԭ���ٶ�
        void Start()
        {
            originalScale = targetTr.localScale; // ����ԭʼ������ֵ

        }
        public void OnTouchBegin()
        {
            // �����ʼʱ���߼�
            ScaleObject(originalScale * scaleMultiplier);
        }

        public void OnTouchEnd()
        {
            // �������ʱ���߼�
            Debug.Log(targetTr.gameObject.name + "ѡ��");
            ScaleObject(originalScale);
            //OnSelected?.Invoke();

        }

        public void OnTouchExit()
        {
            // ���������ѡ��ֱ�ӻ�ԭ���š�
            // ����Э���𽥻�ԭ
            ScaleObject(originalScale);
        }

        private void ScaleObject(Vector3 toScale)
        {
            targetTr.DOScale(toScale, scaleSpeed);
        }
    }
}