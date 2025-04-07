using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserStarLuckPanel : MonoBehaviour
{
    public string initInfo;

    public RawImage xingZuoTexture;
    public Text xingZuoNameText;
    public Text xingZuoDateText;
    public TextMeshProUGUI xingZuoLuckText;
    public GenerateRecommendStar recommendStarContent;
    public StarWheelController starWheelController;

    private JArray recommendStarDateList;

    private void Awake()
    {
        ActionEventHandler.Instance.AddEventListener(GameInfo.userStarLuckInfo_star_Dispatch_Index, Init);
    }

    private void Init(object[] param)
    {
        initInfo = param[0].ToString();
       // Debug.Log("�����������ݣ�" + initInfo);
        PanelShowInit();
    }
 
    public void PanelShowInit()
    {
        if (string.IsNullOrEmpty(initInfo))
        {
            Debug.Log("Unity:����û���û���Ϣ");
            return;
        }
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log(userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);

        // ����ͼ����
        if (userData["sign_img"] != null && !string.IsNullOrEmpty(userData["sign_img"].ToString()))
        {
            string sign_imgUrl = userData["sign_img"].ToString();
            StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(sign_imgUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        }
        //��������
        if (userData["sign_name"] != null)
        {
            xingZuoNameText.text = userData["sign_name"].ToString();
            //ˢ������
            ReflashStarWheel();
        }
        ////��������
        if (userData["sign_date"] != null)
            xingZuoDateText.text = userData["sign_date"].ToString();
        ////��������
        if (userData["sign_content"] != null)
        {
            xingZuoLuckText.text = userData["sign_content"].ToString();
            xingZuoLuckText.GetComponent<AutoResizeText_High>().UpdateText(xingZuoLuckText.text);
        }
        if (userData["recommend_list"] != null && (userData["recommend_list"] as JArray).Count > 0)
        {
            // ��ȡ date_list ����
            recommendStarDateList = userData["recommend_list"] as JArray;

            // ��ȡ����
            //Debug.Log("#" + mainUserStoryDateList.Count);
            recommendStarContent.Clean();
            //�����Ƽ��������ɽű�����������
            recommendStarContent.cecommendStarCount = recommendStarDateList.Count;
            recommendStarContent.CreateRecommendStar(recommendStarDateList);
        }

    }


    public void SetXingZuoBgTexture()
    {
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        // ����ͼ����
        if (userData["sign_img"] != null && !string.IsNullOrEmpty(userData["sign_img"].ToString()))
        {
            string sign_imgUrl = userData["sign_img"].ToString();
            StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(sign_imgUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        }
    }
    public void ReflashStarWheelTexture()
    {
       
                starWheelController.lastSelectedTouchAndInputEffect3DGameobject.OnEnter?.Invoke();
             
    }
    /// <summary>
    /// ˢ����������
    /// </summary>
    public void ReflashStarWheel()
    {
        //���������¼�
        foreach (var item in starWheelController.starWheelsList)
        {
            if (item.forStarName == xingZuoNameText.text)
            {
                starWheelController.lastSelectedTouchAndInputEffect3DGameobject = item;
                ExcludeMethodForOnSelected(item);
                //item.OnEnter?.Invoke();
            }
           
        }
    }

    private void ExcludeMethodForOnSelected(TouchAndInpuEffect3D item,int exCount=4)
    {
       // Debug.Log("@#@");
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

    private void OnTextureDownloadSuccess(Texture2D texture)
    {
        // ����������κ������������飬������ʾͼƬ
       // Debug.Log("����ͼƬ�������.");
        // ���磬���Խ�����Ӧ�õ�һ��������
        // GetComponent<Renderer>().material.mainTexture = texture;
        xingZuoTexture.texture = texture;
    }
    // ����ʧ�ܵĻص�
    private void OnTextureDownloadError(string error)
    {
     //  Debug.Log("Image download failed: " + error);
    }
    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userStarLuckInfo_star_Dispatch_Index, Init);

    }
}
