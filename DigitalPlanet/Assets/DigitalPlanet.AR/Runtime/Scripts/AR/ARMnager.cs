using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TimeStar.Prefabs;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class ARMnager : MonoSingleton<ARMnager>
    {
        public PrefabSpawner prefabSpawner;

        private bool isArCheck;//验证
        private bool isArClockIn;//打卡
        private UserLocation userLocation;//用户位置
        private CapsuleLocation capsuleLocation;//胶囊位置
        private JObject capsule_data_info;

        private int requestCount = 0;
        private const int maxRequestCount = 3;
        private const float requestInterval = 3f;
        private Coroutine locationRequestCoroutine;

        string ss = "{\"page_size\":10," +
                         "\"page_total\":0," +
                         "\"hits_total\":0," +
                         "\"is_join\":1," +
                         "\"attend\":1," +
                         "\"cid\":\"587290a7fec1767301d5bf9e2652651b\"," +
                         "\"oneself\":1," +
                         "\"description\":\"这是一个记忆胶囊，有内容优优文案哈哈哈哈哈\"," +
                         "\"fail_reason\":\"\"," +
                         "\"obj_address\":{" +
                         "\"longitude\":116.4756," +
                         "\"latitude\":39.9058," +
                         "\"province\":\"北京市\"," +
                         "\"province_code\":\"110000\"," +
                         "\"city\":\"北京市\"," +
                         "\"city_code\":\"1101\"," +
                         "\"address_name\":\"海航大厦\"," +
                         "\"address\":\"北京市朝阳区霄云路甲26号\"," +
                         "\"address_tag\":\"房地产;写字楼\"" +
                         "}}";
        //#if UNITY_EDITOR
        //    private void OnGUI()
        //    {
        //        if(GUILayout.Button("asdfasdf"))
        //        {
        //           StartCoroutine(prefabSpawner.DelayedSpawn());
        //        }
        //    }
        //#endif
        private void Start()
        {
            InitAR();
        }

        public void InitAR()
        {
            //prefabSpawner.AsyCreateCapsule("");
            //初始化及检查数据
            if (InitArData())//如果数据OK
            {
                prefabSpawner.AsyCreateCapsule(capsule_data_info.ToString());
                //初始化及检查数据

            }
            //if (InitArData())//如果数据OK
            //{
            //    StopLocationRequest();
            //    CheckHandler checkHandler = new CheckHandler();
            //    if (checkHandler.CalculateDistance(userLocation.latitude, userLocation.longitude,
            //        capsuleLocation.latitude_capsule, capsuleLocation.longitude_capsule) < 500f)
            //    {
            //        //capsule_data_info["position"].ToString();
            //        //capsule_data_info["rotation"].ToString();
            //        //capsule_data_info["id"].ToString();
            //        //capsule_data_info.ToString();

            //        //生成胶囊
            //        prefabSpawner.AsyCreateCapsule(capsule_data_info.ToString());
            //    }
            //    else//如果数据不OK
            //    {
            //        //轮询用户位置信息
            //        Debug.Log("@@大于500米，请返回");
            //        GameObject tish00 = Instantiate(Resources.Load<GameObject>("Canvas"));
            //        tish00.transform.GetComponentInChildren<Text>().text = "大于500米，请返回";

            //    }
            //}
            //else
            //{
            //    Debug.Log("@@有数据报空，重新请求用户位置");
            //    GameObject tish00 = Instantiate(Resources.Load<GameObject>("Canvas"));
            //    tish00.transform.GetComponent<Text>().text = "有数据报空，重新请求用户位置";
            //    //轮询用户位置信息
            //    locationRequestCoroutine = StartCoroutine(PeriodicLocationRequest());
            //}
        }

        /// <summary>
        /// 轮询请求用户位置
        /// </summary>
        /// <returns></returns>
        IEnumerator PeriodicLocationRequest()
        {
            while (requestCount < maxRequestCount)
            {
                RequestUserLocation();
                requestCount++;
                yield return new WaitForSeconds(requestInterval);
            }
            Debug.Log("已达到最大请求次数，停止请求用户位置信息。");
        }
        /// <summary>
        /// 请求到用户数据时调用的轮询停止方法
        /// </summary>
        private void StopLocationRequest()
        {
            if (locationRequestCoroutine != null)
            {
                StopCoroutine(locationRequestCoroutine);
                Debug.Log("已成功获取用户位置信息，停止请求。");
            }
        }
        private bool InitArData()
        {
#if UNITY_EDITOR
            GameInfo.capsule_data = ss;
            GameInfo.user_location = "{\"longitude\":\"116.46711730957031\",\"latitude\":\"39.95938491821289\"}";
#endif
            if (!string.IsNullOrEmpty(GameInfo.user_location))
            {
                JObject user_location = JObject.Parse(GameInfo.user_location);//用户胶囊json对象
                Debug.Log(user_location.ToString());
                userLocation = new UserLocation();
                userLocation.longitude = double.Parse(user_location["longitude"].ToString());
                userLocation.latitude = double.Parse(user_location["latitude"].ToString());

            }
            else
            {
                Debug.Log("@user_location为空");
                return false;
            }
            if (!string.IsNullOrEmpty(GameInfo.capsule_data))
                capsule_data_info = JObject.Parse(GameInfo.capsule_data);//用户胶囊json对象
            else
            {
                Debug.Log("@capsule_data为空");
                return false;
            }
            GameInfo.user_location = "";
            GameInfo.capsule_data = "";
            if (capsule_data_info != null)
            {
                if (capsule_data_info["obj_address"] != null)
                {
                    JObject obj_address = JObject.Parse(capsule_data_info["obj_address"].ToString());
                    Debug.Log("obj_address数据" + obj_address.ToString());
                    capsuleLocation = new CapsuleLocation
                    {
                        longitude_capsule = double.Parse(obj_address["longitude"].ToString()),
                        latitude_capsule = double.Parse(obj_address["latitude"].ToString()),
                    };
                }
                else
                {
                    Debug.Log("@capsule_data+obj_address为空");
                    return false;
                }
            }
            else
            {
                Debug.Log("@capsule_data_info为空");
                return false;
            }
            return true;
        }

        public void RequestUserLocation()
        {
            //请求生成胶囊
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.RequestUserLocation.ToString(),
                data = ""
            };
            string data_main = JsonUtility.ToJson(toNativeData);
            NativeBridge.Instance.SendMessageToNative(data_main);
            Debug.Log($"@请求用户位置信息: {data_main}");
        }

        /// <summary>
        /// 验证AR验证
        /// </summary>
        public void RequestUserARCheck()
        {

#if UNITY_EDITOR
            UserARCheckData userARCheckData = new UserARCheckData
            {
                user_id = GameInfo.tempUser_Id,
                cid = capsule_data_info["cid"].ToString(),
                longitude = userLocation.longitude,
                latitude = userLocation.latitude
            };
#else
        UserARCheckData userARCheckData = new UserARCheckData
        { 
            user_id = GameInfo.User_Id,
            cid = capsule_data_info["cid"].ToString(),
            longitude= userLocation.longitude,
            latitude= userLocation.latitude
        };
#endif
            string param = JsonUtility.ToJson(userARCheckData);
            string needData = GetNeedHttpData(GameInfo.userInfoInterface, param);
            ActionEventHandler.Instance.Dispatch(GameInfo.userInfo_main_Dispatch_Index, needData);
        }

        public string GetNeedHttpData(string interfaceName, string interfaceParam)
        {
#if UNITY_EDITOR
            return HttpTest.HttpTestFunc(interfaceName, interfaceParam);
#else
        return HttpHelper.HttpRequest(interfaceName, interfaceParam);
#endif
        }
    }
}