using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserStarInfoPanel : MonoBehaviour
{
    public string initInfo;

    public RawImage touXiangTexture;
    public TextMeshProUGUI niChengText;
    public TextMeshProUGUI xingZuoShengRiText;
    public string xingZuoName;
    public XingZuoPanel xingZuoPanel;

    public GameObject starReleaseButton;
    private void Awake()
    {
        ActionEventHandler.Instance.AddEventListener(GameInfo.userInfo_star_Dispatch_Index, Init);
    }
    private void Init(object[] param)
    {
        initInfo = param[0].ToString();
        //Debug.Log("�����û����ݣ�" + initInfo);
        UIStateInit();
        PanelShowInit();
    }
    public void UIStateInit()
    {
        
        //if (!GameManager.Instance.isTarget)//�Լ�
        //{
        //    starReleaseButton.SetActive(true);
        //}
        //else
        //{
        //    starReleaseButton.SetActive(false);
        //}
        starReleaseButton.SetActive(true);
    }
    private void PanelShowInit()
    {
        if (string.IsNullOrEmpty(initInfo))
        {
            Debug.Log("untiy:���û���Ϣ");
            return;
        }
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
                                                  // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("untiy:" + userObj["msg"]);
            return;
        }
        string dataJson = userObj["data"].ToString();

        JObject userData = JObject.Parse(dataJson);

        // ͷ������
        string avatarUrl = userData["avatar_url"].ToString();
        StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(avatarUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        //����
        if (userData["user_name"] != null)
        {
            niChengText.text = userData["user_name"].ToString();
            niChengText.GetComponent<AutoResizeText_Width>().UpdateText(niChengText.text);
        }
        //����
        string shengri = "";
        if (userData["birthday"] == null)
        {
            Debug.Log("Unity���û���û������");
            //����native
#if !UNITY_EDITOR
        ToNativeData toNativeData_showTimeStory = new ToNativeData
        {
            command = CommandDataType.CreateUserBirthday.ToString(),
            data =""
        };
        string data = JsonUtility.ToJson(toNativeData_showTimeStory);
        NativeBridge.Instance.SendMessageToNative(data);
#endif
            return;
        }
        if (userData["birthday"] != null)
            shengri = userData["birthday"].ToString();
        //����
        string xingzuo = "";
        if (userData["constellation"] != null)
            xingzuo = userData["constellation"].ToString();

        xingZuoName = xingzuo;
        xingZuoShengRiText.text = "(" + xingzuo + shengri.Split("-")[1] + "." + shengri.Split("-")[2] + ")";
        RefalshXingZuoPlaneTexture();

    }

    public void RefalshXingZuoPlaneTexture()
    {
        if (string.IsNullOrEmpty(xingZuoName)) return;
        string xingzuo = xingZuoName;
        for (int i = 0; i < xingZuoPanel.xingZuoImageList.Count; i++)
        {
            if (xingZuoPanel.xingZuoImageList[i].name.Contains(xingzuo))
            {
                xingZuoPanel.SetXingZuoPlaneTexture(i);
            }
        }
    }

    private void OnTextureDownloadSuccess(Texture2D texture)
    {
        // ����������κ������������飬������ʾͼƬ
      //  Debug.Log("Image downloaded successfully.");
        // ���磬���Խ�����Ӧ�õ�һ��������
        // GetComponent<Renderer>().material.mainTexture = texture;
        touXiangTexture.texture = texture;
    }
    // ����ʧ�ܵĻص�
    private void OnTextureDownloadError(string error)
    {
      //  Debug.LogError("Image download failed: " + error);
    }

   


    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userInfo_star_Dispatch_Index, Init);
    }
}
