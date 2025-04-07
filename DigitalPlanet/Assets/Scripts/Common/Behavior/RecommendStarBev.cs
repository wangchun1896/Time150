using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecommendStarBev : MonoBehaviour
{
    public string recommendStarDetal;
    private Text recommendStarTagText;
    private Button recommendButton;
    JObject recommendStarInfo;

    public TMP_InputField searchInput;
    public StarWheelController starWheelController;
    private void Awake()
    {
        searchInput = FindObjectOfType<TMP_InputField>();
        recommendStarTagText = transform.Find("星座标签Text").GetComponent<Text>();
        recommendButton= transform.Find("星座标签Text").GetComponent<Button>();
        recommendButton.onClick.AddListener(OnRecommendButtonClick);
    }
    private void OnDestroy()
    {
        if (recommendStarInfo != null)
            recommendStarInfo = null;
        recommendButton.onClick.RemoveListener(OnRecommendButtonClick);
        searchInput = null;
        recommendStarTagText = null;
        recommendButton = null;
    }
    public void Init(StarWheelController starWheelController_)
    {
        starWheelController = starWheelController_;
    }

    private void OnRecommendButtonClick()
    {
        //赋值到input
        searchInput.text = recommendStarTagText.text.Split("(")[1].Split(")")[0];
        //触发轮盘事件
        //foreach (var item in starWheelController.starWheelsList)
        //{
        //    if (item.forStarName == gameObject.name)
        //        item.OnSelected?.Invoke();
        //}
        ReflashStarWheel();
        //根据星座刷运势
        RefreshLuckByStar();
        //根据标签刷胶囊
        RefreshCapsuleByTag();

    }

     /// <summary>
    /// 刷新星座轮盘
    /// </summary>
    public void ReflashStarWheel()
    {
        //触发轮盘事件
        foreach (var item in starWheelController.starWheelsList)
        {
            if (item.forStarName == gameObject.name)
                //这里不能再执行轮盘自己的刷新事件
                ExcludeMethodForOnSelected(item);
            //item.OnSelected?.Invoke();
        }
    }
    private void ExcludeMethodForOnSelected(TouchAndInpuEffect3D item,int exCount=4)
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
    /// <summary>
    /// 根据星座刷运势
    /// </summary>
    public void RefreshLuckByStar()
    {
         if(!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserStarLuckData_star_ByStar(recommendStarTagText.text.Split("(")[0]);
         else//游客
            GameManager.Instance.RequestUserStarLuckData_star_ByStar_target(recommendStarTagText.text.Split("(")[0]);

    }
    /// <summary>
    /// 根据标签刷胶囊
    /// </summary>
    public void RefreshCapsuleByTag()
    {
        string starName = recommendStarTagText.text.Split("(")[0];
        string content = recommendStarTagText.text.Split("(")[1].Split(")")[0];
        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStarAndTag(starName, content);
        else//游客
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStarAndTag_target(starName, content);


        //string starName = recommendStarTagText.text.Split("(")[0];
        //List<string> tags = new List<string>();
        //tags.Add(recommendStarTagText.text.Split("(")[1].Split(")")[0]);
        //GameManager.Instance.RequestUserTimeCapsuleData_star_ByStarAndTag(starName, tags);
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(recommendStarDetal)) return;
        recommendStarInfo = JObject.Parse(recommendStarDetal);
        gameObject.name = recommendStarInfo["sign_name"].ToString();
        InitItem();
    }

    public void InitItem()
    {
        recommendStarTagText.text = recommendStarInfo["sign_name"].ToString() + "(" + recommendStarInfo["tag_name"] + ")";
    }

   
}
