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
                    gameObject.name = "���ID:" + adObj["aid"].ToString();
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
            Debug.Log("���ͼ����󣬼��URL: " + error);
        }
        private void OnAdTextureDownloadSuccess(Texture2D texture)
        {
            // ����������κ������������飬������ʾͼƬ
            // Debug.Log("����ͼƬ���سɹ�.");
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
            // Debug.Log("������@" + adInfo);

            //��ʼ����������Native���ͳ�����Ϣ
            if (string.IsNullOrEmpty(adInfo)) Debug.LogError("�����ϸ��ϢΪ��");
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
            // ��ȡ��ǰ�󶨵��¼�����
            int eventCount = item.OnSelected.GetPersistentEventCount();
            // ����Ҫִ�е��¼����������Ϊ 4
            int numberOfEventsToExecute = Mathf.Min(eventCount, exCount);
            // ����ǰ 4 ���¼���ִ������
            for (int i = 0; i < numberOfEventsToExecute; i++)
            {
                var target = item.OnSelected.GetPersistentTarget(i);
                var methodName = item.OnSelected.GetPersistentMethodName(i);
                // ȷ��Ŀ�겻Ϊ null�����ҷ�������Ϊ null
                if (target != null && !string.IsNullOrEmpty(methodName))
                {
                    // �ҵ�Ŀ������еķ���������
                    var methodInfo = target.GetType().GetMethod(methodName);
                    if (methodInfo != null)
                    {
                        // ���÷���
                        object[] objs = new object[1] { item.transform.GetSiblingIndex() };
                        methodInfo.Invoke(target, objs);
                    }
                }
            }
        }


    }
}