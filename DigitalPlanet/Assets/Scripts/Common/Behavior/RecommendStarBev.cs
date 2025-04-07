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
        recommendStarTagText = transform.Find("������ǩText").GetComponent<Text>();
        recommendButton= transform.Find("������ǩText").GetComponent<Button>();
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
        //��ֵ��input
        searchInput.text = recommendStarTagText.text.Split("(")[1].Split(")")[0];
        //���������¼�
        //foreach (var item in starWheelController.starWheelsList)
        //{
        //    if (item.forStarName == gameObject.name)
        //        item.OnSelected?.Invoke();
        //}
        ReflashStarWheel();
        //��������ˢ����
        RefreshLuckByStar();
        //���ݱ�ǩˢ����
        RefreshCapsuleByTag();

    }

     /// <summary>
    /// ˢ����������
    /// </summary>
    public void ReflashStarWheel()
    {
        //���������¼�
        foreach (var item in starWheelController.starWheelsList)
        {
            if (item.forStarName == gameObject.name)
                //���ﲻ����ִ�������Լ���ˢ���¼�
                ExcludeMethodForOnSelected(item);
            //item.OnSelected?.Invoke();
        }
    }
    private void ExcludeMethodForOnSelected(TouchAndInpuEffect3D item,int exCount=4)
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
    /// <summary>
    /// ��������ˢ����
    /// </summary>
    public void RefreshLuckByStar()
    {
         if(!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserStarLuckData_star_ByStar(recommendStarTagText.text.Split("(")[0]);
         else//�ο�
            GameManager.Instance.RequestUserStarLuckData_star_ByStar_target(recommendStarTagText.text.Split("(")[0]);

    }
    /// <summary>
    /// ���ݱ�ǩˢ����
    /// </summary>
    public void RefreshCapsuleByTag()
    {
        string starName = recommendStarTagText.text.Split("(")[0];
        string content = recommendStarTagText.text.Split("(")[1].Split(")")[0];
        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStarAndTag(starName, content);
        else//�ο�
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
