using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInfoPanel : MonoBehaviour
{
    private string initInfo;//用户面板是用户信息
    public MeshRenderer xingZuoPlane;//星座透明片
    public RawImage xingZhuTouXiangTexture;
    public Text xingQiuDengJiText;
    public TextMeshProUGUI niChengText;
    public TextMeshProUGUI xingQiuMingChengText;
    public GameObject returnUserButton;
    public List<Texture2D> xingZuoTextureList;//星座透明片需要更换的图片List
    public UserFuncPanel userFuncPanel;
    private void Awake()
    {
        xingZhuTouXiangTexture = transform.Find("用户详细信息面板/星主头像").GetComponent<RawImage>();
        xingQiuDengJiText = transform.Find("用户详细信息面板/星球等级/星球等级Txet").GetComponent<Text>();
        niChengText = transform.Find("用户详细信息面板/昵称").GetComponent<TextMeshProUGUI>();
        xingQiuMingChengText = transform.Find("用户详细信息面板/星球名称").GetComponent<TextMeshProUGUI>();
        ActionEventHandler.Instance.AddEventListener(GameInfo.userInfo_main_Dispatch_Index, Init);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userFocusInfo_main_Dispatch_Index, FocusInit);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userAddFriendInfo_main_Dispatch_Index, AddFriendInit);

    }

   

    private void UIStateInit_user()
    {   //用户进入
        //开启的
        returnUserButton.SetActive(true);
        JObject userObj = JObject.Parse(initInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if(userData["is_read_notice"]==null)
        {
            userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(false);//无新消息
        }
        else
        {
            switch (userData["is_read_notice"].ToString())
            {
                case "False":
                    userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(false);//无新消息
                    break;
                case "True":
                    userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(true);//无新消息
                    break;
                default:
                    break;
            }
        }
        userFuncPanel.xiaoXiButton.SetActive(true);
       //关闭的
        returnUserButton.SetActive(false);
        userFuncPanel.guanZhuButton.SetActive(false);
        userFuncPanel.yiGuanZhuButton.SetActive(false);
        userFuncPanel.huiGuanButton.SetActive(false);
        userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        userFuncPanel.jiaHaoYouButton.SetActive(false);
        userFuncPanel.siLiaoAnNiu.SetActive(false);
    }
    private void UIStateInit_target()
    {//目标进入
        //开启的
        returnUserButton.SetActive(true);
        JObject userObj = JObject.Parse(initInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        //if(userData["follow_status"]!=null)//关注状态
        //{
        //    switch (userData["follow_status"].ToString())
        //    {
        //        case "0"://0:未关注
        //            userFuncPanel.guanZhuButton.SetActive(true);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "1"://1:已关注
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(true);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "2"://2:回关
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(true);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "3"://3:互相关注
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //if (userData["is_friend"] != null)//关注状态
        //{
        //    Debug.Log("is_friend:" + userData["is_friend"].ToString());
        //    switch (userData["is_friend"].ToString())
        //    {
        //        case "False":
        //            userFuncPanel.jiaHaoYouButton.SetActive(true);
        //        break;
        //        case "True":
        //            userFuncPanel.jiaHaoYouButton.SetActive(false);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        if (userData["third_user_id"] != null)
           GameInfo.User_Id_TT = userData["third_user_id"].ToString();
        //开启私聊
        //userFuncPanel.siLiaoAnNiu.SetActive(true);

        //关闭的
        userFuncPanel.xiaoXiButton.SetActive(false);
    }
    private void Init(object[] param)
    {
        initInfo = param[0].ToString();
       // Debug.Log("main用户信息数据" + initInfo);
        PanelShowInit();

        if (!GameManager.Instance.isTarget)
            UIStateInit_user();
        else
            UIStateInit_target();
    }
    /// <summary>
    /// 界面显示初始化
    /// </summary>
    private void PanelShowInit()
    {
        JObject userObj = JObject.Parse(initInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);

        // 头像设置
        string avatarUrl = userData["avatar_url"].ToString();
        StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(avatarUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        //名称
        if(userData["user_name"]!=null)
        niChengText.text = userData["user_name"].ToString()+ "（星主）";
        //星球名称
        if (userData["goods_name"] != null)
            xingQiuMingChengText.text = userData["goods_name"].ToString();
        //星主等级
        if (userData["level"] != null)
            xingQiuDengJiText.text ="Lv."+ userData["level"].ToString();
    }
    //关注按钮方法
    public void OnFollowButtonClick(string buttonName)
    {
        GameManager.Instance.RequestUserFocusData_main_target(buttonName);
    }
    //回关按钮方法
    public void OnFollowBackButtonClick(string buttonName)
    {
        GameManager.Instance.RequestUserFocusData_main_target(buttonName);
    }
    private void FocusInit(object[] param)//关注信息提取
    {
       string focusInfo = param[0].ToString();
        string buttonName = param[1].ToString();
        JObject focusObj = JObject.Parse(focusInfo);//提取命令
        Debug.Log("关注返回消息：" + focusInfo);
        if(focusObj["code"]!=null)
        {
            if(focusObj["code"].ToString()=="000")
            {
                switch (buttonName)
                {
                    case "关注按钮":
                        Debug.Log("关注按钮");
                        userFuncPanel.guanZhuButton.SetActive(false);
                        break;
                    case "回关按钮":
                        Debug.Log("回关按钮");
                        userFuncPanel.huiGuanButton.SetActive(false);
                        userFuncPanel.huXiangGuanZhuButton.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
    //加好友按钮方法
    public void OnAddFriendButtonClick()
    {
        GameManager.Instance.RequestUserAddFriendData_main_target();
    }
    private void AddFriendInit(object[] param)
    {
        string addFriendInfo = param[0].ToString();
        JObject addFriendObj = JObject.Parse(addFriendInfo);//提取命令
        Debug.Log("加好友返回消息：" + addFriendInfo);
        if (addFriendObj["code"] != null)
        {
            if (addFriendObj["code"].ToString() == "000")
            {
                Debug.Log("加好友按钮");
                userFuncPanel.jiaHaoYouButton.SetActive(false);
            }
        }
    }

    // 下载成功的回调
    private void OnTextureDownloadSuccess(Texture2D texture)
    {
        // 这里可以做任何你想做的事情，比如显示图片
       // Debug.Log("Image downloaded successfully.");
        // 例如，可以将纹理应用到一个对象上
        // GetComponent<Renderer>().material.mainTexture = texture;
        xingZhuTouXiangTexture.texture = texture;
    }
    public  Sprite TextureToSpriteConverter(Texture2D texture)
    {
        // 将 Texture2D 转换为 Sprite
        // 参数: Rectangle的左下角宽度和高度，可以使用texture的整个尺寸
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    // 下载失败的回调
    private void OnTextureDownloadError(string error)
    {
        Debug.LogError("Image download failed: " + error);
    }

    public string GetNeedHttpData(string interfaceName,string interfaceParam)
    {
#if UNITY_EDITOR
        return HttpTest.HttpTestFunc(interfaceName, interfaceParam);
#else
        return HttpHelper.HttpRequest(interfaceName, interfaceParam);
#endif
    }
    public void SetXingZuoPlaneTexture(string xingZuoName)
    {
        Texture2D xingZuoTex=null ;
        foreach (var item in xingZuoTextureList)
        {
            if (item.name.Contains(xingZuoName))
                xingZuoTex = item;
        }
        xingZuoPlane.material.DOFade(0, 0.5f).OnComplete(() =>
        {
            xingZuoPlane.material.SetTexture("_BaseMap", xingZuoTex);
            xingZuoPlane.material.DOFade(1, 0.5f);
        });

    }

    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userInfo_main_Dispatch_Index, Init);
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userFocusInfo_main_Dispatch_Index, FocusInit);
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userAddFriendInfo_main_Dispatch_Index, AddFriendInit);
    }
}
