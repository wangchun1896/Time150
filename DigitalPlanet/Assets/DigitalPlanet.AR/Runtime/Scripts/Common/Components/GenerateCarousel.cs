using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Newtonsoft.Json.Linq;
namespace TimeStar.DigitalPlant
{
    public class GenerateCarousel : MonoBehaviour
    {
        public RectTransform content; // Content 的 RectTransform
        public float displayTime = 5f; // 每个 Item 的展示时间
        public float transitionTime = 0.5f; // 切换到下一个 Item 的过渡时间

        public GameObject adItem;
        public int adCount = 4;
        private int currentIndex = 0; // 当前展示的 Item 索引
        public RectTransform[] items; // 存储所有 Item 的 RectTransform

        public void CreateAd(JArray adListDateList = null)
        {
            if (adListDateList == null || adListDateList.Count == 0) return;
            for (int i = 0; i < adListDateList.Count; i++)
            {
                GameObject Item_ad = Instantiate(adItem);
                Item_ad.transform.parent = transform;
                //添加可点击代码
                Item_ad.transform.localScale = Vector3.one;
                //添加胶囊信息类
                AdBev adb = Item_ad.AddComponent<AdBev>();

                adb.adInfo = adListDateList[i].ToString();

            }
            StartLunBo();
        }
        private void StartLunBo()
        {
            // 获取所有子 Item 并存储到数组中
            items = new RectTransform[content.childCount];
            for (int i = 0; i < content.childCount; i++)
            {
                items[i] = content.GetChild(i).GetComponent<RectTransform>();
            }

            StartCoroutine(CarouselRoutine());
        }

        private IEnumerator CarouselRoutine()
        {
            while (true) // 无限循环
            {
                // 展示当前 Item
                yield return new WaitForSeconds(displayTime);

                // 计算下一个 Item 的索引
                int nextIndex = (currentIndex + 1) % items.Length;

                // 计算目标位置
                Vector3 targetPosition = new Vector3(-nextIndex * items[currentIndex].rect.width, 0, 0);

                // 使用 DOTween 来平滑过渡到下一个 Item
                content.DOLocalMove(targetPosition, transitionTime).SetEase(Ease.InOutSine);

                // 更新当前索引
                currentIndex = nextIndex;

                // 等待过渡完成后，再继续展示下一个 Item
                yield return new WaitForSeconds(transitionTime);
            }
        }

        public void CleanAD()
        {
            // 检查  是否为空
            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // 销毁子物体
                }
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}