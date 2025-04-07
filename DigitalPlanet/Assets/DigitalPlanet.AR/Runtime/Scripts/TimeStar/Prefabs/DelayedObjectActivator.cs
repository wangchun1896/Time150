using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DelayedObjectActivator : MonoBehaviour
{
    [Serializable]
    public class ActivationConfig
    {
        public string objectName = "ARModel";
        public float delaySeconds = 2f;
    }

    [Tooltip("配置要延迟激活的对象")]
    public List<ActivationConfig> activationConfigs = new List<ActivationConfig>();

    void Awake()
    {
        // 如果没有配置，添加默认配置
        if (activationConfigs.Count == 0)
        {
            activationConfigs.Add(new ActivationConfig());
        }
    }

    void Start()
    {
        // 为每个配置启动协程
        foreach (var config in activationConfigs)
        {
            StartCoroutine(ActivateObjectWithDelay(config));
        }
    }

    IEnumerator ActivateObjectWithDelay(ActivationConfig config)
    {
        // 查找所有匹配名称的游戏对象
        GameObject[] targetObjects = FindObjectsOfType<GameObject>(true)
            .Where(obj => obj.name == config.objectName)
            .ToArray();

        if (targetObjects.Length == 0)
        {
            Debug.LogWarning($"未找到名为 {config.objectName} 的游戏对象");
            yield break;
        }

        // 等待指定的延迟时间
        yield return new WaitForSeconds(config.delaySeconds);

        // 激活所有匹配的对象
        foreach (var obj in targetObjects)
        {
            obj.SetActive(true);
            Debug.Log($"已激活对象: {obj.name}");
        }
    }
}