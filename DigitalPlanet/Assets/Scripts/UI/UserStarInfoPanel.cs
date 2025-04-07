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
        //Debug.Log("星座用户数据：" + initInfo);
        UIStateInit();
        PanelShowInit();
    }
    public void UIStateInit()
    {
        
        //if (!GameManager.Instance.isTarget)//自己
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
            Debug.Log("untiy:无用户信息");
            return;
        }
        JObject userObj = JObject.Parse(initInfo);//提取命令
                                                  // 取出 data 字段，并将其反序列化
        if (userObj["data"] == null)
        {
            Debug.Log("untiy:" + userObj["msg"]);
            return;
        }
        string dataJson = userObj["data"].ToString();

        JObject userData = JObject.Parse(dataJson);

        // 头像设置
        string avatarUrl = userData["avatar_url"].ToString();
        StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(avatarUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        //名称
        if (userData["user_name"] != null)
        {
            niChengText.text = userData["user_name"].ToString();
            niChengText.GetComponent<AutoResizeText_Width>().UpdateText(niChengText.text);
        }
        //生日
        string shengri = "";
        if (userData["birthday"] == null)
        {
            Debug.Log("Unity：用户还没有生日");
            //吊起native
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
        //星座
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
        // 这里可以做任何你想做的事情，比如显示图片
      //  Debug.Log("Image downloaded successfully.");
        // 例如，可以将纹理应用到一个对象上
        // GetComponent<Renderer>().material.mainTexture = texture;
        touXiangTexture.texture = texture;
    }
    // 下载失败的回调
    private void OnTextureDownloadError(string error)
    {
      //  Debug.LogError("Image download failed: " + error);
    }

   


    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userInfo_star_Dispatch_Index, Init);
    }
}
