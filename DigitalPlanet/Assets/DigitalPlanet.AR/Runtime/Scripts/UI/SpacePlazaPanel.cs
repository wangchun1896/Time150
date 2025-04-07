using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class SpacePlazaPanel : MonoBehaviour
    {
        public string advertisementInitInfo;
        private JArray userAdDateList;
        public GenerateCarousel adContent;
        private void Awake()
        {
            ActionEventHandler.Instance.AddEventListener(GameInfo.userAdvertisementInfo_main_Dispatch_Index, AdvertisementInfoInit);
        }

        private void AdvertisementInfoInit(object[] param)
        {
            if (!string.IsNullOrEmpty(advertisementInitInfo)) return;
            advertisementInitInfo = param[0].ToString();
            //Debug.Log("������ݣ�" + advertisementInitInfo);

            InitAD();
        }

        private void InitAD()
        {
            if (string.IsNullOrEmpty(advertisementInitInfo))
            {
                Debug.Log("Unity���޹������");
                return;
            }
            JObject advertisementObj = JObject.Parse(advertisementInitInfo);//��ȡ����
                                                                            // ȡ�� data �ֶΣ������䷴���л�
            string dataJson = advertisementObj["data"].ToString();
            JObject userAdData = JObject.Parse(dataJson);
            if (userAdData["show_type"] == null)
            {
                Debug.Log("Unity���������Ϊ��");
                return;
            }
            if (int.Parse(userAdData["show_type"].ToString()) == 1)
            {
                if (userAdData["advertisements"] == null)
                {
                    Debug.Log("Unity������ֶ�Ϊ��");
                    return;
                }
                else
                {

                    userAdDateList = userAdData["advertisements"] as JArray;
                    if (userAdDateList.Count == 0)
                    {
                        Debug.Log("Unity������ֶ��б�����Ϊ0");
                        return;
                    }

                }
            }
            else
            {
                Debug.Log("Unity��������Ͳ��ǹ���");
                return;
            }

            //���ɹ��
            adContent.CleanAD();
            adContent.adCount = userAdDateList.Count;
            adContent.CreateAd(userAdDateList);
            //Debug.Log("@" + mainUserCapsulsDateList.Count);
            //generateCapsulesObj.CreateJiaoNang(mainUserCapsulsDateList);

        }

        private void OnDestroy()
        {
            ActionEventHandler.Instance.RemoveEventListener(GameInfo.userAdvertisementInfo_main_Dispatch_Index, AdvertisementInfoInit);
        }
    }
}