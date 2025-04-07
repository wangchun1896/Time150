using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class ScrollRectHandler : MonoBehaviour
    {
        public ScrollRect scrollRect;

        private bool canTrigger = true; // ���ƴ����ı�־
        public float threshold = 0.1f; // ��������ֵ�����ڱ���С�Ļ���������ʶ��

        private void Start()
        {
            scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        private void OnScrollRectValueChanged(Vector2 scrollPosition)
        {
            // ��ȡ ScrollRect �����ݺ��ӿڵĸ߶�
            float contentHeight = scrollRect.content.rect.height;
            float viewportHeight = scrollRect.viewport.rect.height;

            // �жϹ����Ƿ��˵ײ�
            if (scrollPosition.y <= 0 && scrollRect.vertical)
            {
                // ����û��Ƿ�������������
                if (scrollPosition.y < -threshold && canTrigger)
                {
                    // �����¼�
                    Debug.Log("User is pulling up at the bottom!");

                    // ���ñ�־λΪ false����ֹ�ظ�����
                    canTrigger = false;
                    StartCoroutine(AsyOpenCanTrigger());
                }
            }
            else
            {
                // ����û�û���ڵײ������ñ�־λ
                //  canTrigger = true;
            }
        }

        private IEnumerator AsyOpenCanTrigger()
        {
            yield return new WaitForSeconds(1);
            canTrigger = true;
        }
    }
}