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
        public RectTransform content; // Content �� RectTransform
        public float displayTime = 5f; // ÿ�� Item ��չʾʱ��
        public float transitionTime = 0.5f; // �л�����һ�� Item �Ĺ���ʱ��

        public GameObject adItem;
        public int adCount = 4;
        private int currentIndex = 0; // ��ǰչʾ�� Item ����
        public RectTransform[] items; // �洢���� Item �� RectTransform

        public void CreateAd(JArray adListDateList = null)
        {
            if (adListDateList == null || adListDateList.Count == 0) return;
            for (int i = 0; i < adListDateList.Count; i++)
            {
                GameObject Item_ad = Instantiate(adItem);
                Item_ad.transform.parent = transform;
                //��ӿɵ������
                Item_ad.transform.localScale = Vector3.one;
                //��ӽ�����Ϣ��
                AdBev adb = Item_ad.AddComponent<AdBev>();

                adb.adInfo = adListDateList[i].ToString();

            }
            StartLunBo();
        }
        private void StartLunBo()
        {
            // ��ȡ������ Item ���洢��������
            items = new RectTransform[content.childCount];
            for (int i = 0; i < content.childCount; i++)
            {
                items[i] = content.GetChild(i).GetComponent<RectTransform>();
            }

            StartCoroutine(CarouselRoutine());
        }

        private IEnumerator CarouselRoutine()
        {
            while (true) // ����ѭ��
            {
                // չʾ��ǰ Item
                yield return new WaitForSeconds(displayTime);

                // ������һ�� Item ������
                int nextIndex = (currentIndex + 1) % items.Length;

                // ����Ŀ��λ��
                Vector3 targetPosition = new Vector3(-nextIndex * items[currentIndex].rect.width, 0, 0);

                // ʹ�� DOTween ��ƽ�����ɵ���һ�� Item
                content.DOLocalMove(targetPosition, transitionTime).SetEase(Ease.InOutSine);

                // ���µ�ǰ����
                currentIndex = nextIndex;

                // �ȴ�������ɺ��ټ���չʾ��һ�� Item
                yield return new WaitForSeconds(transitionTime);
            }
        }

        public void CleanAD()
        {
            // ���  �Ƿ�Ϊ��
            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // ����������
                }
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}