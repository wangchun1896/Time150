using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoSizeBase : MonoBehaviour
{
    // ����һ���¼�������֪ͨ�߶ȱ仯
    public UnityEvent onHeightChange;

    protected virtual void Awake()
    {
        // ��ʼ�� UnityEvent
        onHeightChange = onHeightChange ?? new UnityEvent();
    }

    public virtual void CalculateAndSetHeight()
    {
        // ��������������ʵ�־����߼�
    }
}
