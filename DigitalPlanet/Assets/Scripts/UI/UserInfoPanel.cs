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
    private string initInfo;//�û�������û���Ϣ
    public MeshRenderer xingZuoPlane;//����͸��Ƭ
    public RawImage xingZhuTouXiangTexture;
    public Text xingQiuDengJiText;
    public TextMeshProUGUI niChengText;
    public TextMeshProUGUI xingQiuMingChengText;
    public GameObject returnUserButton;
    public List<Texture2D> xingZuoTextureList;//����͸��Ƭ��Ҫ������ͼƬList
    public UserFuncPanel userFuncPanel;
    private void Awake()
    {
        xingZhuTouXiangTexture = transform.Find("�û���ϸ��Ϣ���/����ͷ��").GetComponent<RawImage>();
        xingQiuDengJiText = transform.Find("�û���ϸ��Ϣ���/����ȼ�/����ȼ�Txet").GetComponent<Text>();
        niChengText = transform.Find("�û���ϸ��Ϣ���/�ǳ�").GetComponent<TextMeshProUGUI>();
        xingQiuMingChengText = transform.Find("�û���ϸ��Ϣ���/��������").GetComponent<TextMeshProUGUI>();
        ActionEventHandler.Instance.AddEventListener(GameInfo.userInfo_main_Dispatch_Index, Init);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userFocusInfo_main_Dispatch_Index, FocusInit);
        ActionEventHandler.Instance.AddEventListener(GameInfo.userAddFriendInfo_main_Dispatch_Index, AddFriendInit);

    }

   

    private void UIStateInit_user()
    {   //�û�����
        //������
        returnUserButton.SetActive(true);
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if(userData["is_read_notice"]==null)
        {
            userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(false);//������Ϣ
        }
        else
        {
            switch (userData["is_read_notice"].ToString())
            {
                case "False":
                    userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(false);//������Ϣ
                    break;
                case "True":
                    userFuncPanel.xiaoXiButton.transform.GetChild(0).gameObject.SetActive(true);//������Ϣ
                    break;
                default:
                    break;
            }
        }
        userFuncPanel.xiaoXiButton.SetActive(true);
       //�رյ�
        returnUserButton.SetActive(false);
        userFuncPanel.guanZhuButton.SetActive(false);
        userFuncPanel.yiGuanZhuButton.SetActive(false);
        userFuncPanel.huiGuanButton.SetActive(false);
        userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        userFuncPanel.jiaHaoYouButton.SetActive(false);
        userFuncPanel.siLiaoAnNiu.SetActive(false);
    }
    private void UIStateInit_target()
    {//Ŀ�����
        //������
        returnUserButton.SetActive(true);
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        //if(userData["follow_status"]!=null)//��ע״̬
        //{
        //    switch (userData["follow_status"].ToString())
        //    {
        //        case "0"://0:δ��ע
        //            userFuncPanel.guanZhuButton.SetActive(true);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "1"://1:�ѹ�ע
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(true);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "2"://2:�ع�
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(true);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(false);
        //            break;
        //        case "3"://3:�����ע
        //            userFuncPanel.guanZhuButton.SetActive(false);
        //            userFuncPanel.yiGuanZhuButton.SetActive(false);
        //            userFuncPanel.huiGuanButton.SetActive(false);
        //            userFuncPanel.huXiangGuanZhuButton.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //if (userData["is_friend"] != null)//��ע״̬
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
        //����˽��
        //userFuncPanel.siLiaoAnNiu.SetActive(true);

        //�رյ�
        userFuncPanel.xiaoXiButton.SetActive(false);
    }
    private void Init(object[] param)
    {
        initInfo = param[0].ToString();
       // Debug.Log("main�û���Ϣ����" + initInfo);
        PanelShowInit();

        if (!GameManager.Instance.isTarget)
            UIStateInit_user();
        else
            UIStateInit_target();
    }
    /// <summary>
    /// ������ʾ��ʼ��
    /// </summary>
    private void PanelShowInit()
    {
        JObject userObj = JObject.Parse(initInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);

        // ͷ������
        string avatarUrl = userData["avatar_url"].ToString();
        StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(avatarUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        //����
        if(userData["user_name"]!=null)
        niChengText.text = userData["user_name"].ToString()+ "��������";
        //��������
        if (userData["goods_name"] != null)
            xingQiuMingChengText.text = userData["goods_name"].ToString();
        //�����ȼ�
        if (userData["level"] != null)
            xingQiuDengJiText.text ="Lv."+ userData["level"].ToString();
    }
    //��ע��ť����
    public void OnFollowButtonClick(string buttonName)
    {
        GameManager.Instance.RequestUserFocusData_main_target(buttonName);
    }
    //�عذ�ť����
    public void OnFollowBackButtonClick(string buttonName)
    {
        GameManager.Instance.RequestUserFocusData_main_target(buttonName);
    }
    private void FocusInit(object[] param)//��ע��Ϣ��ȡ
    {
       string focusInfo = param[0].ToString();
        string buttonName = param[1].ToString();
        JObject focusObj = JObject.Parse(focusInfo);//��ȡ����
        Debug.Log("��ע������Ϣ��" + focusInfo);
        if(focusObj["code"]!=null)
        {
            if(focusObj["code"].ToString()=="000")
            {
                switch (buttonName)
                {
                    case "��ע��ť":
                        Debug.Log("��ע��ť");
                        userFuncPanel.guanZhuButton.SetActive(false);
                        break;
                    case "�عذ�ť":
                        Debug.Log("�عذ�ť");
                        userFuncPanel.huiGuanButton.SetActive(false);
                        userFuncPanel.huXiangGuanZhuButton.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
    //�Ӻ��Ѱ�ť����
    public void OnAddFriendButtonClick()
    {
        GameManager.Instance.RequestUserAddFriendData_main_target();
    }
    private void AddFriendInit(object[] param)
    {
        string addFriendInfo = param[0].ToString();
        JObject addFriendObj = JObject.Parse(addFriendInfo);//��ȡ����
        Debug.Log("�Ӻ��ѷ�����Ϣ��" + addFriendInfo);
        if (addFriendObj["code"] != null)
        {
            if (addFriendObj["code"].ToString() == "000")
            {
                Debug.Log("�Ӻ��Ѱ�ť");
                userFuncPanel.jiaHaoYouButton.SetActive(false);
            }
        }
    }

    // ���سɹ��Ļص�
    private void OnTextureDownloadSuccess(Texture2D texture)
    {
        // ����������κ������������飬������ʾͼƬ
       // Debug.Log("Image downloaded successfully.");
        // ���磬���Խ�����Ӧ�õ�һ��������
        // GetComponent<Renderer>().material.mainTexture = texture;
        xingZhuTouXiangTexture.texture = texture;
    }
    public  Sprite TextureToSpriteConverter(Texture2D texture)
    {
        // �� Texture2D ת��Ϊ Sprite
        // ����: Rectangle�����½ǿ�Ⱥ͸߶ȣ�����ʹ��texture�������ߴ�
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    // ����ʧ�ܵĻص�
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
