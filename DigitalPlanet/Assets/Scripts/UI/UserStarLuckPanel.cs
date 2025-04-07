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
       // Debug.Log("星座运势数据：" + initInfo);
        PanelShowInit();
    }
 
    public void PanelShowInit()
    {
        if (string.IsNullOrEmpty(initInfo))
        {
            Debug.Log("Unity:星座没有用户信息");
            return;
        }
        JObject userObj = JObject.Parse(initInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        if (userObj["data"] == null)
        {
            Debug.Log(userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);

        // 星座图设置
        if (userData["sign_img"] != null && !string.IsNullOrEmpty(userData["sign_img"].ToString()))
        {
            string sign_imgUrl = userData["sign_img"].ToString();
            StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(sign_imgUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        }
        //星座名称
        if (userData["sign_name"] != null)
        {
            xingZuoNameText.text = userData["sign_name"].ToString();
            //刷新轮盘
            ReflashStarWheel();
        }
        ////星座周期
        if (userData["sign_date"] != null)
            xingZuoDateText.text = userData["sign_date"].ToString();
        ////星座运势
        if (userData["sign_content"] != null)
        {
            xingZuoLuckText.text = userData["sign_content"].ToString();
            xingZuoLuckText.GetComponent<AutoResizeText_High>().UpdateText(xingZuoLuckText.text);
        }
        if (userData["recommend_list"] != null && (userData["recommend_list"] as JArray).Count > 0)
        {
            // 获取 date_list 数组
            recommendStarDateList = userData["recommend_list"] as JArray;

            // 获取数量
            //Debug.Log("#" + mainUserStoryDateList.Count);
            recommendStarContent.Clean();
            //设置推荐星座生成脚本的生成数量
            recommendStarContent.cecommendStarCount = recommendStarDateList.Count;
            recommendStarContent.CreateRecommendStar(recommendStarDateList);
        }

    }


    public void SetXingZuoBgTexture()
    {
        JObject userObj = JObject.Parse(initInfo);//提取命令
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        // 星座图设置
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
    /// 刷新星座轮盘
    /// </summary>
    public void ReflashStarWheel()
    {
        //触发轮盘事件
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

    private void OnTextureDownloadSuccess(Texture2D texture)
    {
        // 这里可以做任何你想做的事情，比如显示图片
       // Debug.Log("星座图片下载完毕.");
        // 例如，可以将纹理应用到一个对象上
        // GetComponent<Renderer>().material.mainTexture = texture;
        xingZuoTexture.texture = texture;
    }
    // 下载失败的回调
    private void OnTextureDownloadError(string error)
    {
     //  Debug.Log("Image download failed: " + error);
    }
    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userStarLuckInfo_star_Dispatch_Index, Init);

    }
}
