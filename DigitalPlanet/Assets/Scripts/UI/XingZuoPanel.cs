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
    public MeshRenderer xingZuoPlane;//星座透明片
    public List<Texture2D> xingZuoTextureList;//星座透明片需要更换的图片List
    public Transform xingZuoDiZuo;//星座底座
    public List<Vector3> xingZuoDiZuoRotateEularList;//星座底座旋转欧拉角List
    public MeshRenderer diZuoFuHaoPlane;//星座透明片
    public List<Texture2D> diZuoFuHaoTextureList;//星座透明片需要更换的图片List
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
      //  Debug.Log("星座胶囊数据：" + initInfo);
        StarCapsuleInit();
    }
    private void StarCapsuleInit()
    {
        if (string.IsNullOrEmpty(initInfo))
        {
            Debug.Log("Unity:星座没有胶囊数据信息");
            return;
        }
        JObject xingZuoTimeCapsuleInfo = JObject.Parse(initInfo);//提取命令
        // 取出 data 字段，并将其反序列化
        if (xingZuoTimeCapsuleInfo["data"] == null)
        {
            Debug.Log(xingZuoTimeCapsuleInfo["msg"].ToString());
            return;
        }
        string dataJson = xingZuoTimeCapsuleInfo["data"].ToString();
        JObject xingZuoTimeCapsuleData = JObject.Parse(dataJson);


        //显示胶囊
        // 获取 date_list 数组
        if (xingZuoTimeCapsuleData["date_list"] == null)
        {
            Debug.Log("根据星座获取的胶囊数量为0");
            return;
        }
        starUserCapsulsDateList = xingZuoTimeCapsuleData["date_list"] as JArray;

        // 获取数量
        //Debug.Log("#" + mainUserCapsulsDateList.Count);
        //设置胶囊生成脚本的生成数量
        GenerateCapsulesOnVertices generateCapsulesObj = starJiaoNangBall.GetComponent<GenerateCapsulesOnVertices>();
        generateCapsulesObj.CleanCapsules();
        generateCapsulesObj.jiaoNangCount = starUserCapsulsDateList.Count;
       // Debug.Log("@胶囊数量：" + starUserCapsulsDateList.Count);
        generateCapsulesObj.CreateJiaoNang(starUserCapsulsDateList);


    }

    /// <summary>
    /// 转盘根据星座刷运势
    /// </summary>
    public void RefreshLuckByStar_Wheel(TouchAndInpuEffect3D clickObj)
    {
        string starName = clickObj.forStarName;
        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserStarLuckData_star_ByStar(starName);
        else//游客
            GameManager.Instance.RequestUserStarLuckData_star_ByStar_target(starName);
    }
    /// <summary>
    /// 转盘根据标签刷胶囊
    /// </summary>
    public void RefreshCapsuleByTag_Wheel(TouchAndInpuEffect3D clickObj)
    {
        //性别sex
        int sex = 0;
        foreach (var item in sexToggleList)
        {
            if(item.isOn)
            {
                sex = item.transform.GetSiblingIndex();
                break;
            }
        }
        //输入框content
        inputContent.text = "";
        string content = inputContent.text;
        //年龄age
        string age = "";
        foreach (var item in ageToggleList)
        {
            if (item.isOn)
            {
                age = item.gameObject.name;
                break;
            }
        }
        //星座
      //  Debug.Log("age:" + age);
        string starName = string.IsNullOrEmpty(starWheelController.currentSeletedStar) ?
            userStarInfoPanel.xingZuoName : starWheelController.currentSeletedStar;
        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age(starName, content, sex, age);
        else
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age_target(starName, content, sex, age);
    }


    public void RefreshCapsuleByTag_Search()
    {
        //性别sex
        int sex = 0;
        foreach (var item in sexToggleList)
        {
            if (item.isOn)
            {
                sex = item.transform.GetSiblingIndex();
                break;
            }
        }
        //输入框content
        string content = inputContent.text;
        //年龄age
        string age = "";
        foreach (var item in ageToggleList)
        {
            if (item.isOn)
            {
                age = item.gameObject.name;
                break;
            }
        }
        //星座 //starWheelController
        string starName = string.IsNullOrEmpty(starWheelController.currentSeletedStar)? 
            userStarInfoPanel.xingZuoName: starWheelController.currentSeletedStar;

        if (!GameManager.Instance.isTarget)//自己
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age(starName, content, sex, age);
        else
            GameManager.Instance.RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age_target(starName, content, sex, age);
    }
    #region 星座轮盘等逻辑
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
                //Debug.Log("轮盘选择：" + xingZuoImageList[i].name.Split('_')[1]);
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
