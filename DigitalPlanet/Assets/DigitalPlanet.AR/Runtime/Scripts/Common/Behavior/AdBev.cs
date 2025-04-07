using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TimeStar.DigitalPlant
{
    public class AdBev : MonoBehaviour
    {
        public RawImage adTexture;
        public Button adButton;
        public string adInfo;
        JObject adObj;


        private void Awake()
        {
            adTexture = GetComponent<RawImage>();
            adButton = GetComponent<Button>();
            adButton.onClick.AddListener(OnAdButtonClick);
        }
        private void OnDestroy()
        {
            if (adObj != null)
                adObj = null;
            adButton.onClick.RemoveListener(OnAdButtonClick);
            adTexture = null;
            adButton = null;
        }
        private void Start()
        {
            AdInit();
        }

        private void AdInit()
        {
            if (!string.IsNullOrEmpty(adInfo))
            {
                adObj = JObject.Parse(adInfo);
            }
            if (adObj != null)
            {
                if (adObj["aid"] != null)
                {
                    gameObject.name = "广告ID:" + adObj["aid"].ToString();
                }
                if (adObj["url_img"] != null)
                {
                    string url_img = adObj["url_img"].ToString();
                    StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(url_img, OnAdTextureDownloadSuccess, OnAdTextureDownloadError));

                }
            }
        }

        private void OnAdTextureDownloadError(string error)
        {
            Debug.Log("广告图像错误，检查URL: " + error);
        }
        private void OnAdTextureDownloadSuccess(Texture2D texture)
        {
            // 这里可以做任何你想做的事情，比如显示图片
            // Debug.Log("故事图片下载成功.");
            if (texture != null)
            {
                //Debug.Log(timeStoryTexture.gameObject.name + "--");
                //timeStoryTexture.texture = texture;
                //if (timeStoryTexture.GetComponent<AutoSizeRawImage>() != null)
                //    timeStoryTexture.GetComponent<AutoSizeRawImage>().SetImageTexture(texture);
                adTexture.texture = texture;
            }
            else
            {
            }
        }

        private void OnAdButtonClick()
        {
            // Debug.Log("点击广告@" + adInfo);

            //初始化场景后向Native发送场景信息
            if (string.IsNullOrEmpty(adInfo)) Debug.LogError("广告详细信息为空");
#if !UNITY_EDITOR
        ToNativeData toNativeData_showAd = new ToNativeData
        {
            command = CommandDataType.ShowAD.ToString(),
            data = adInfo
        };
        string data = JsonUtility.ToJson(toNativeData_showAd);
        NativeBridge.Instance.SendMessageToNative(data);
#endif

        }


        private void ExcludeMethodForOnSelected(TouchAndInpuEffect3D item, int exCount = 4)
        {
            // 获取当前绑定的事件数量
            int eventCount = item.OnSelected.GetPersistentEventCount();
            // 计算要执行的事件数量，最多为 4
            int numberOfEventsToExecute = Mathf.Min(eventCount, exCount);
            // 遍历前 4 个事件并执行它们
            for (int i = 0; i < numberOfEventsToExecute; i++)
            {
                var target = item.OnSelected.GetPersistentTarget(i);
                var methodName = item.OnSelected.GetPersistentMethodName(i);
                // 确保目标不为 null，并且方法名不为 null
                if (target != null && !string.IsNullOrEmpty(methodName))
                {
                    // 找到目标对象中的方法并调用
                    var methodInfo = target.GetType().GetMethod(methodName);
                    if (methodInfo != null)
                    {
                        // 调用方法
                        object[] objs = new object[1] { item.transform.GetSiblingIndex() };
                        methodInfo.Invoke(target, objs);
                    }
                }
            }
        }


    }
}