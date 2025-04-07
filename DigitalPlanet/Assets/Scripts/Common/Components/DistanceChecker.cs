using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public Transform targetObject; // Ҫ�������Ŀ������
    public GameObject targetTag;
    public string capsleDetal;

    private float timer; // ��ʱ��
    public float triggerInterval = 3.0f; // �������ʱ��
    private bool hasTriggeredOnce = false; // ��־��������ʾ�Ƿ��Ѿ�������һ��
    public void Init(Transform target,GameObject tag,string capsleDetal_)
    {
        targetObject = target;
        targetTag = tag;
        capsleDetal = capsleDetal_;



    }
    private void Update()
    {
        // ���Ŀ�������Ƿ��ѱ�����
        if (targetObject != null)
        {
            // ���㵱ǰ������Ŀ������֮��ľ���
            float distance = Vector3.Distance(transform.position, targetObject.position);

            // �������С�� 0.05
            if (distance < 0.05f)
            {
                // �����һ�δ���������ִ��
                if (!hasTriggeredOnce)
                {
                    TriggerAction();
                    hasTriggeredOnce = true; // ���ñ�־����ʾ�Ѿ�������һ��
                }
                else
                {
                    // ���¼�ʱ��
                    timer += Time.deltaTime;

                    // �����ʱ����ֵ���ڵ��ڴ������
                    if (timer >= triggerInterval)
                    {
                        TriggerAction(); // �ٴ�ִ�д����߼�
                        timer = 0f; // ���ü�ʱ��
                    }
                }
            }
            else
            {
                // ���������� 0.05�����ü�ʱ���ͱ�־
                timer = 0f;
                hasTriggeredOnce = false; // Ҳ����Ҫȡ���Ѿ�������־
            }
        }
        else
        {
            // Debug.LogWarning("Ŀ������δ���ã�");
        }
    }


    private void TriggerAction()
    {
        // ���Ŀ���ǩ�Ƿ�δ���ڼ���״̬
        if (!targetTag.activeInHierarchy)
        {
            targetTag.GetComponent<CapsuleTagController>().capsuleDetail = capsleDetal;
            targetTag.SetActive(true);
        }
    }
}
