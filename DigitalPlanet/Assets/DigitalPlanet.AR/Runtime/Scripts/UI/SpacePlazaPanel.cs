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
            //Debug.Log("广告数据：" + advertisementInitInfo);

            InitAD();
        }

        private void InitAD()
        {
            if (string.IsNullOrEmpty(advertisementInitInfo))
            {
                Debug.Log("Unity：无广告数据");
                return;
            }
            JObject advertisementObj = JObject.Parse(advertisementInitInfo);//提取命令
                                                                            // 取出 data 字段，并将其反序列化
            string dataJson = advertisementObj["data"].ToString();
            JObject userAdData = JObject.Parse(dataJson);
            if (userAdData["show_type"] == null)
            {
                Debug.Log("Unity：广告类型为空");
                return;
            }
            if (int.Parse(userAdData["show_type"].ToString()) == 1)
            {
                if (userAdData["advertisements"] == null)
                {
                    Debug.Log("Unity：广告字段为空");
                    return;
                }
                else
                {

                    userAdDateList = userAdData["advertisements"] as JArray;
                    if (userAdDateList.Count == 0)
                    {
                        Debug.Log("Unity：广告字段列表数量为0");
                        return;
                    }

                }
            }
            else
            {
                Debug.Log("Unity：广告类型不是滚动");
                return;
            }

            //生成广告
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