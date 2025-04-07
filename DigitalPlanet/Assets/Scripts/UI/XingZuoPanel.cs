using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XingZuoPanel : MonoBehaviour
{
    public string initInfo;
    public List<GameObject> xingZuoImageList;
    public MeshRenderer xingZuoPlane;//����͸��Ƭ
    public List<Texture2D> xingZuoTextureList;//����͸��Ƭ��Ҫ������ͼƬList
    public Transform xingZuoDiZuo;//��������
    public List<Vector3> xingZuoDiZuoRotateEularList;//����������תŷ����List
    public MeshRenderer diZuoFuHaoPlane;//����͸��Ƭ
    public List<Texture2D> diZuoFuHaoTextureList;//����͸��Ƭ��Ҫ������ͼƬList
    private JArray starUserCapsulsDateList;
    public GameObject starJiaoNangBall;

    public List<Toggle> sexToggleList;
    public TMP_InputField inputContent;
    public List<Toggle> ageToggleList;

    public UserStarInfoPanel userStarInfoPanel;
    public StarWheelController starWheelController;

    private void Awake()
    {
        ActionEventHandler.Instance.AddEventListener(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, Init);
    }

    private void Init(object[] param)
    {
        initInfo = param[0].ToString();
      //  Debug.Log("�����������ݣ�" + initInfo);
        StarCapsuleInit();
    }
    private void StarCapsuleInit()
    {
        if (string.IsNullOrEmpty(initInfo))
        {
            Debug.Log("Unity:����û�н���������Ϣ");
            return;
        }
        JObject xingZuoTimeCapsuleInfo = JObject.Parse(initInfo);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (xingZuoTimeCapsuleInfo["data"] == null)
        {
            Debug.Log(xingZuoTimeCapsuleInfo["msg"].ToString());
            return;
        }
        string dataJson = xingZuoTimeCapsuleInfo["data"].ToString();
        JObject xingZuoTimeCapsuleData = JObject.Parse(dataJson);


        //��ʾ����
        // ��ȡ date_list ����
        if (xingZuoTimeCapsuleData["date_list"] == null)
        {
            Debug.Log("����������ȡ�Ľ�������Ϊ0");
            return;
        }
        starUserCapsulsDateList = xingZuoTimeCapsuleData["date_list"] as JArray;

        // ��ȡ����
        //Debug.Log("#" + mainUserCapsulsDateList.Count);
        //���ý������ɽű�����������
        GenerateCapsulesOnVertices generateCapsulesObj = starJiaoNangBall.GetComponent<GenerateCapsulesOnVertices>();
        generateCapsulesObj.CleanCapsules();
        generateCapsulesObj.jiaoNangCount = starUserCapsulsDateList.Count;
       // Debug.Log("@����������" + starUserCapsulsDateList.Count);
        generateCapsulesObj.CreateJiaoNang(starUserCapsulsDateList);


    }

    /// <summary>
    /// ת�̸�������ˢ����
    /// </summary>
    public void RefreshLuckByStar_Wheel(TouchAndInpuEffect3D clickObj)
    {
        string starName = clickObj.forStarName;
        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserStarLuckData_star_ByStar(starName);
        else//�ο�
            GameManager.Instance.RequestUserStarLuckData_star_ByStar_target(starName);
    }
    /// <summary>
    /// ת�̸��ݱ�ǩˢ����
    /// </summary>
    public void RefreshCapsuleByTag_Wheel(TouchAndInpuEffect3D clickObj)
    {
        //�Ա�sex
        int sex = 0;
        foreach (var item in sexToggleList)
        {
            if(item.isOn)
            {
                sex = item.transform.GetSiblingIndex();
                break;
            }
        }
        //�����content
        inputContent.text = "";
        string content = inputContent.text;
        //����age
        string age = "";
        foreach (var item in ageToggleList)
        {
            if (item.isOn)
            {
                age = item.gameObject.name;
                break;
            }
        }
        //����
      //  Debug.Log("age:" + age);
        string starName = string.IsNullOrEmpty(starWheelController.currentSeletedStar) ?
            userStarInfoPanel.xingZuoName : starWheelController.currentSeletedStar;
        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age(starName, content, sex, age);
        else
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age_target(starName, content, sex, age);
    }


    public void RefreshCapsuleByTag_Search()
    {
        //�Ա�sex
        int sex = 0;
        foreach (var item in sexToggleList)
        {
            if (item.isOn)
            {
                sex = item.transform.GetSiblingIndex();
                break;
            }
        }
        //�����content
        string content = inputContent.text;
        //����age
        string age = "";
        foreach (var item in ageToggleList)
        {
            if (item.isOn)
            {
                age = item.gameObject.name;
                break;
            }
        }
        //���� //starWheelController
        string starName = string.IsNullOrEmpty(starWheelController.currentSeletedStar)? 
            userStarInfoPanel.xingZuoName: starWheelController.currentSeletedStar;

        if (!GameManager.Instance.isTarget)//�Լ�
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age(starName, content, sex, age);
        else
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age_target(starName, content, sex, age);
    }
    #region �������̵��߼�
    public void SetDiZuoFuHaoPlaneTexture(int index)
    {
        diZuoFuHaoPlane.material.DOFade(0, 0.5f).OnComplete(() =>
        {
            diZuoFuHaoPlane.material.SetTexture("_BaseMap", diZuoFuHaoTextureList[index]);
            diZuoFuHaoPlane.material.DOFade(1, 0.5f);
        });

    }
    public void SetXingZuoPlaneTexture(int index)
    {
        xingZuoPlane.material.DOFade(0, 0.5f).OnComplete(() =>
        {
            xingZuoPlane.material.SetTexture("_BaseMap", xingZuoTextureList[index]);
            xingZuoPlane.material.DOFade(1, 0.5f);
        });
       
    }
    public void SetXingZuoDiZuoRotateEular(int index)
    {
        xingZuoDiZuo.DOLocalRotateQuaternion(Quaternion.Euler(xingZuoDiZuoRotateEularList[index]), 0.5f);
    }
    public void XingZuoLunPanShow(int index)
    {
        for (int i = 0; i < xingZuoImageList.Count; i++)
        {
            if(i==index)
                xingZuoImageList[i].SetActive(true);
            else
                xingZuoImageList[i].SetActive(false);
        }
    }

    public void XingZuoWheelImageAllClose()
    {
        for (int i = 0; i < xingZuoImageList.Count; i++)
        {
            xingZuoImageList[i].SetActive(false);
        }
    }
    public void XingZuoSearchAllInit()
    {
        for (int i = 0; i < xingZuoImageList.Count; i++)
        {
            xingZuoImageList[i].SetActive(false);

        }
        FindObjectOfType<TMP_InputField>().text="";
        for (int i = 0; i < ageToggleList.Count; i++)
        {
            if (i == 0)
                ageToggleList[i].isOn = true;
            else
                ageToggleList[i].isOn = false;
        }
        for (int i = 0; i < sexToggleList.Count; i++)
        {
            if (i == 0)
                sexToggleList[i].isOn = true;
            else
                sexToggleList[i].isOn = false;
        }

    }
    public void XingZuoLunPanSelected(int index)
    {
        for (int i = 0; i < xingZuoImageList.Count; i++)
        {
            if (i == index)
            {
                xingZuoImageList[i].SetActive(true);
                //Debug.Log("����ѡ��" + xingZuoImageList[i].name.Split('_')[1]);
                starWheelController.currentSeletedStar = xingZuoImageList[i].name.Split('_')[1];
            }
            else
                xingZuoImageList[i].SetActive(false);
        }
    }

    #endregion
    private void OnDestroy()
    {
        ActionEventHandler.Instance.RemoveEventListener(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, Init);
    }
}
