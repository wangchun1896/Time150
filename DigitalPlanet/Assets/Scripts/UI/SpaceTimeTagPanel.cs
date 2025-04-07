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
    private string initTimeCapsuleInfo;//ʱ�ս���
    private string initTimeStoryInfo;//ʱ�����
    private JArray mainUserCapsulsDateList;
    private JArray mainUserStoryDateList;

    private void Awake()
    {
        shiKongJiaoNangToggle.onValueChanged.AddListener(OnShiKongCapsuleToggleValueChanged);//ʱ�ս���Toggle
        shiGuangGuShiToggle.onValueChanged.AddListener(OnShiGuangStoryToggleValueChanged);//ʱ�����Toggle
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
        //Debug.Log("��ǰ���û�->"+ GameManager.Instance.isTarget);
        //ˢ������
        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserTimeCapsuleData_main();
        else
            GameManager.Instance.RequestUserTimeCapsuleData_main_target();
        yield return new WaitForSeconds(0.1f);
        //������
        //���³�ʼ��
        CapsulePanelShowInit();
        shiKongJiaoNangToggle.isOn = true;
    }
   
    private void RefreshTimeStory(object[] param=null)
    {
        StartCoroutine(AsyRefreshTimeStory());
    }
    private IEnumerator AsyRefreshTimeStory()
    {
        //ˢ������
        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserTimeStory_main();
        else
            GameManager.Instance.RequestUserTimeStory_main_target();
        yield return new WaitForSeconds(0.04f);
        //�������
        //���³�ʼ��
        StoryPanelShowInit();
    }
    private void InitTimeCapsule(object[] param)
    {
        initTimeCapsuleInfo = param[0].ToString();
        if (!GameManager.Instance.isTarget)//�Լ�
        {
            shiKongCapusleButton.SetActive(true);
            shiGuangStoryButton.SetActive(true);
        }
        else
        {
            shiKongCapusleButton.SetActive(false);
            shiGuangStoryButton.SetActive(false);
        }
           // Debug.Log("ʱ�ս������ݣ�" + initTimeCapsuleInfo);
    }
    private void InitTimeStory(object[] param)
    {
        initTimeStoryInfo = param[0].ToString();
        if (!GameManager.Instance.isTarget)//�Լ�
        {
            shiGuangStoryButton.SetActive(true);
        }
        else
        {
            shiGuangStoryButton.SetActive(false);
        }
        //Debug.Log("ʱ��������ݣ�" + initTimeStoryInfo);
    }
    public void OnStarReturnMain()
    {
        if (shiKongJiaoNangToggle.isOn)
            ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, "");
    }
    /// <summary>
    /// ���ҽ�����ʾ��ʼ��
    /// </summary>
    private void CapsulePanelShowInit()
    {
        GenerateCapsulesOnVertices generateCapsulesObj = mainJiaoNangBall.GetComponent<GenerateCapsulesOnVertices>();
        if (string.IsNullOrEmpty(initTimeCapsuleInfo))
        {
            Debug.Log("Unity���޽���");
            generateCapsulesObj.CleanCapsules();
            nullPanel.SetActive(true);
            return;
        }
        JObject userCapsuleObj = JObject.Parse(initTimeCapsuleInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        string dataJson = userCapsuleObj["data"].ToString();
        JObject userCapsuleData = JObject.Parse(dataJson);
        // ��ȡ date_list ����
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

        // ��ȡ����
       // Debug.Log("#" + mainUserCapsulsDateList.Count);
        //���ý������ɽű�����������
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
            Debug.Log("untiy:���û�����");
            timeStoryPanel.CleanStory();
            return;
        }
        JObject userTimeStoryObj = JObject.Parse(initTimeStoryInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        string dataJson = userTimeStoryObj["data"].ToString();
        JObject userCapsuleData = JObject.Parse(dataJson);
        // ��ȡ date_list ����
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
        // ��ȡ����
        //Debug.Log("#" + mainUserStoryDateList.Count);
        //���ý������ɽű�����������
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
    /// ʱ�ս���Toggle
    /// </summary>
    /// <param name="isOn"></param>
    private void OnShiKongCapsuleToggleValueChanged(bool isOn)
    {
      //  Debug.Log(shiKongJiaoNangToggle.name + ":" + shiKongJiaoNangToggle.isOn);
        if (isOn)
        {
            CapsulePanelShowInit();
            // ���� Toggle ѡ��״̬ʱ���߼�
            mainJiaoNangBall.SetActive(true);
            shiKongCapusleButton.transform.DOScaleY(1, 0.5f);
            if (AssetBundleLoader.Instance != null)
                AssetBundleLoader.Instance.UnloadUnusedResources();
            // shiKongCapusleButton.SetActive(true);
        }
        else
        {
            // ���� Toggle δѡ��״̬ʱ���߼�
            mainJiaoNangBall.SetActive(false);
           // shiKongCapusleButton.SetActive(false);
            shiKongCapusleButton.transform.DOScaleY(0, 0f);

        }
    }
    /// <summary>
    /// ʱ�����Toggle
    /// </summary>
    /// <param name="isOn"></param>
    private void OnShiGuangStoryToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            jiaoNangTag.SetActive(false);
            StoryPanelShowInit();
            // ���� Toggle ѡ��״̬ʱ���߼�
            shiGuangGuShiPanel.SetActive(true);
            //shiGuangStoryButton.SetActive(true);
            shiGuangStoryButton.transform.DOScaleY(1, 0.5f);
            if (AssetBundleLoader.Instance != null)
                AssetBundleLoader.Instance.UnloadUnusedResources();
        }
        else
        {
            // ���� Toggle δѡ��״̬ʱ���߼�
            shiGuangGuShiPanel.SetActive(false);
           // shiGuangStoryButton.SetActive(false);
            shiGuangStoryButton.transform.DOScaleY(0, 0f);
        }
    }
   

    void OnDestroy()
    {
        // ȷ���ڶ�������ʱ�Ƴ�������
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
