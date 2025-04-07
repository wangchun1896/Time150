using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceTimeTagPanel : MonoBehaviour
{
    public Toggle shiKongJiaoNangToggle;
    public Toggle shiGuangGuShiToggle;

    public GameObject mainJiaoNangBall;
    public GameObject shiGuangGuShiPanel;
    public GameObject shiKongCapusleButton;
    public GameObject shiGuangStoryButton;

    public GameObject nullPanel;
    public GameObject jiaoNangTag;
    private string initTimeCapsuleInfo;//时空胶囊
    private string initTimeStoryInfo;//时光故事
    private JArray mainUserCapsulsDateList;
    private JArray mainUserStoryDateList;

    private void Awake()
    {
        shiKongJiaoNangToggle.onValueChanged.AddListener(OnShiKongCapsuleToggleValueChanged);//时空胶囊Toggle
        shiGuangGuShiToggle.onValueChanged.AddListener(OnShiGuangStoryToggleValueChanged);//时光故事Toggle
        ActionEventHandler.Instance.AddEventListener(GameInfo.userTimeCapsuleInfo_main_Dispatch_Index, InitTimeCapsule);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userTimeStoryInfo_main_Dispatch_Index, InitTimeStory);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, RefreshTimeCapsule);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userTimeStoryInfoRefresh_main_Dispatch_Index, RefreshTimeStory);

       
    }

    private void RefreshTimeCapsule(object[] param=null)
    {
        StartCoroutine(AsyRefreshTimeCapsule());
    }
    private IEnumerator AsyRefreshTimeCapsule()
    {
        //Debug.Log("当前是用户->"+ GameManager.Instance.isTarget);
        //刷新数据
        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserTimeCapsuleData_main();
        else
            GameManager.Instance.RequestUserTimeCapsuleData_main_target();
        yield return new WaitForSeconds(0.1f);
        //清理胶囊
        //重新初始化
        CapsulePanelShowInit();
        shiKongJiaoNangToggle.isOn = true;
    }
   
    private void RefreshTimeStory(object[] param=null)
    {
        StartCoroutine(AsyRefreshTimeStory());
    }
    private IEnumerator AsyRefreshTimeStory()
    {
        //刷新数据
        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserTimeStory_main();
        else
            GameManager.Instance.RequestUserTimeStory_main_target();
        yield return new WaitForSeconds(0.04f);
        //清理故事
        //重新初始化
        StoryPanelShowInit();
    }
    private void InitTimeCapsule(object[] param)
    {
        initTimeCapsuleInfo = param[0].ToString();
        if (!GameManager.Instance.isTarget)//自己
        {
            shiKongCapusleButton.SetActive(true);
            shiGuangStoryButton.SetActive(true);
        }
        else
        {
            shiKongCapusleButton.SetActive(false);
            shiGuangStoryButton.SetActive(false);
        }
           // Debug.Log("时空胶囊数据：" + initTimeCapsuleInfo);
    }
    private void InitTimeStory(object[] param)
    {
        initTimeStoryInfo = param[0].ToString();
        if (!GameManager.Instance.isTarget)//自己
        {
            shiGuangStoryButton.SetActive(true);
        }
        else
        {
            shiGuangStoryButton.SetActive(false);
        }
        //Debug.Log("时光故事数据：" + initTimeStoryInfo);
    }
    public void OnStarReturnMain()
    {
        if (shiKongJiaoNangToggle.isOn)
            ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, "");
    }
    /// <summary>
    /// 胶囊界面显示初始化
    /// </summary>
    private void CapsulePanelShowInit()
    {
        GenerateCapsulesOnVertices generateCapsulesObj = mainJiaoNangBall.GetComponent<GenerateCapsulesOnVertices>();
        if (string.IsNullOrEmpty(initTimeCapsuleInfo))
        {
            Debug.Log("Unity：无胶囊");
            generateCapsulesObj.CleanCapsules();
            nullPanel.SetActive(true);
            return;
        }
        JObject userCapsuleObj = JObject.Parse(initTimeCapsuleInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        string dataJson = userCapsuleObj["data"].ToString();
        JObject userCapsuleData = JObject.Parse(dataJson);
        // 获取 date_list 数组
        if(userCapsuleData["date_list"]==null)
        {
            generateCapsulesObj.CleanCapsules();
            nullPanel.SetActive(true);
            return;
        }
        mainUserCapsulsDateList = userCapsuleData["date_list"] as JArray;
        if (mainUserCapsulsDateList.Count==0)
        {
            generateCapsulesObj.CleanCapsules();
            nullPanel.SetActive(true);
            return;
        }
        nullPanel.SetActive(false);

        // 获取数量
       // Debug.Log("#" + mainUserCapsulsDateList.Count);
        //设置胶囊生成脚本的生成数量
        generateCapsulesObj.CleanCapsules();
        generateCapsulesObj.jiaoNangCount = mainUserCapsulsDateList.Count;
       // Debug.Log("@" + mainUserCapsulsDateList.Count);
        generateCapsulesObj.CreateJiaoNang(mainUserCapsulsDateList);
        //foreach (var item in mainUserCapsulsDateList)
        //{
        //    Debug.Log("##" + item);
        //}


    }

    private void StoryPanelShowInit()
    {
        TimeStoryPanel timeStoryPanel = shiGuangGuShiPanel.GetComponent<TimeStoryPanel>();
        if (string.IsNullOrEmpty(initTimeStoryInfo))
        {
            Debug.Log("untiy:无用户故事");
            timeStoryPanel.CleanStory();
            return;
        }
        JObject userTimeStoryObj = JObject.Parse(initTimeStoryInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        string dataJson = userTimeStoryObj["data"].ToString();
        JObject userCapsuleData = JObject.Parse(dataJson);
        // 获取 date_list 数组
        if (userCapsuleData["date_list"] == null)
        {
            timeStoryPanel.CleanStory();
            nullPanel.SetActive(true);
            return;
        }
        mainUserStoryDateList = userCapsuleData["date_list"] as JArray;

        if (mainUserStoryDateList.Count == 0)
        {
            timeStoryPanel.CleanStory();
            nullPanel.SetActive(true);
            return;
        }
        nullPanel.SetActive(false);
        // 获取数量
        //Debug.Log("#" + mainUserStoryDateList.Count);
        //设置胶囊生成脚本的生成数量
        timeStoryPanel.timeStory_All_Content.GetComponent<RectTransform>().sizeDelta = new Vector2(
            timeStoryPanel.timeStory_All_Content.GetComponent<RectTransform>().sizeDelta.x, 0);
        timeStoryPanel.CleanStory();
        timeStoryPanel.storyCount = mainUserStoryDateList.Count;
        timeStoryPanel.CreateStory(mainUserStoryDateList);
        //foreach (var item in mainUserStoryDateList)
        //{
        //    Debug.Log("##" + item);
        //}


    }
    public string GetNeedHttpData(string interfaceName, string interfaceParam)
    {
#if UNITY_EDITOR
        return HttpTest.HttpTestFunc(interfaceName, interfaceParam);
#else
        return HttpHelper.HttpRequest(interfaceName, interfaceParam);
#endif
    }
    /// <summary>
    /// 时空胶囊Toggle
    /// </summary>
    /// <param name="isOn"></param>
    private void OnShiKongCapsuleToggleValueChanged(bool isOn)
    {
      //  Debug.Log(shiKongJiaoNangToggle.name + ":" + shiKongJiaoNangToggle.isOn);
        if (isOn)
        {
            CapsulePanelShowInit();
            // 处理 Toggle 选中状态时的逻辑
            mainJiaoNangBall.SetActive(true);
            shiKongCapusleButton.transform.DOScaleY(1, 0.5f);
            if (AssetBundleLoader.Instance != null)
                AssetBundleLoader.Instance.UnloadUnusedResources();
            // shiKongCapusleButton.SetActive(true);
        }
        else
        {
            // 处理 Toggle 未选中状态时的逻辑
            mainJiaoNangBall.SetActive(false);
           // shiKongCapusleButton.SetActive(false);
            shiKongCapusleButton.transform.DOScaleY(0, 0f);

        }
    }
    /// <summary>
    /// 时光故事Toggle
    /// </summary>
    /// <param name="isOn"></param>
    private void OnShiGuangStoryToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            jiaoNangTag.SetActive(false);
            StoryPanelShowInit();
            // 处理 Toggle 选中状态时的逻辑
            shiGuangGuShiPanel.SetActive(true);
            //shiGuangStoryButton.SetActive(true);
            shiGuangStoryButton.transform.DOScaleY(1, 0.5f);
            if (AssetBundleLoader.Instance != null)
                AssetBundleLoader.Instance.UnloadUnusedResources();
        }
        else
        {
            // 处理 Toggle 未选中状态时的逻辑
            shiGuangGuShiPanel.SetActive(false);
           // shiGuangStoryButton.SetActive(false);
            shiGuangStoryButton.transform.DOScaleY(0, 0f);
        }
    }
   

    void OnDestroy()
    {
        // 确保在对象销毁时移除监听器
        if (shiKongJiaoNangToggle != null)
        {
            shiKongJiaoNangToggle.onValueChanged.RemoveListener(OnShiKongCapsuleToggleValueChanged);
        }
        if (shiGuangGuShiToggle != null)
        {
            shiGuangGuShiToggle.onValueChanged.RemoveListener(OnShiGuangStoryToggleValueChanged);
        }
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userTimeCapsuleInfo_main_Dispatch_Index, InitTimeCapsule);
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userTimeStoryInfo_main_Dispatch_Index, InitTimeStory);
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, RefreshTimeCapsule);
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userTimeStoryInfoRefresh_main_Dispatch_Index, RefreshTimeStory);
    }





}
