using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class ButtonController : MonoBehaviour
    {
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f; // 目标放大倍数
        public Transform targetTr;
        public float scaleSpeed = 2f; // 放大和还原的速度
        void Start()
        {
            originalScale = targetTr.localScale; // 保存原始的缩放值

        }
        public void OnTouchBegin()
        {
            // 点击开始时的逻辑
            ScaleObject(originalScale * scaleMultiplier);
        }

        public void OnTouchEnd()
        {
            // 点击结束时的逻辑
            Debug.Log(targetTr.gameObject.name + "选择");
            ScaleObject(originalScale);
            //OnSelected?.Invoke();

        }

        public void OnTouchExit()
        {
            // 在这里可以选择直接还原缩放。
            // 启动协程逐渐还原
            ScaleObject(originalScale);
        }

        private void ScaleObject(Vector3 toScale)
        {
            targetTr.DOScale(toScale, scaleSpeed);
        }
    }
}