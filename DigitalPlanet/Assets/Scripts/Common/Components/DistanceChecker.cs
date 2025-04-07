using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public Transform targetObject; // 要检查距离的目标物体
    public GameObject targetTag;
    public string capsleDetal;

    private float timer; // 定时器
    public float triggerInterval = 3.0f; // 触发间隔时间
    private bool hasTriggeredOnce = false; // 标志变量，表示是否已经触发过一次
    public void Init(Transform target,GameObject tag,string capsleDetal_)
    {
        targetObject = target;
        targetTag = tag;
        capsleDetal = capsleDetal_;



    }
    private void Update()
    {
        // 检查目标物体是否已被分配
        if (targetObject != null)
        {
            // 计算当前物体与目标物体之间的距离
            float distance = Vector3.Distance(transform.position, targetObject.position);

            // 如果距离小于 0.05
            if (distance < 0.05f)
            {
                // 如果第一次触发，立即执行
                if (!hasTriggeredOnce)
                {
                    TriggerAction();
                    hasTriggeredOnce = true; // 设置标志，表示已经触发过一次
                }
                else
                {
                    // 更新计时器
                    timer += Time.deltaTime;

                    // 如果计时器的值大于等于触发间隔
                    if (timer >= triggerInterval)
                    {
                        TriggerAction(); // 再次执行触发逻辑
                        timer = 0f; // 重置计时器
                    }
                }
            }
            else
            {
                // 如果距离大于 0.05，重置计时器和标志
                timer = 0f;
                hasTriggeredOnce = false; // 也许需要取消已经触发标志
            }
        }
        else
        {
            // Debug.LogWarning("目标物体未设置！");
        }
    }


    private void TriggerAction()
    {
        // 检查目标标签是否未处于激活状态
        if (!targetTag.activeInHierarchy)
        {
            targetTag.GetComponent<CapsuleTagController>().capsuleDetail = capsleDetal;
            targetTag.SetActive(true);
        }
    }
}
