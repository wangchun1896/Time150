using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoSizeBase : MonoBehaviour
{
    // 定义一个事件，用于通知高度变化
    public UnityEvent onHeightChange;

    protected virtual void Awake()
    {
        // 初始化 UnityEvent
        onHeightChange = onHeightChange ?? new UnityEvent();
    }

    public virtual void CalculateAndSetHeight()
    {
        // 可以在派生类中实现具体逻辑
    }
}
