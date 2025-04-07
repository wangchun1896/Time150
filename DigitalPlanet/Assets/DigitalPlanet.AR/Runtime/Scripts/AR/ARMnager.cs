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

        private bool isArCheck;//��֤
        private bool isArClockIn;//��
        private UserLocation userLocation;//�û�λ��
        private CapsuleLocation capsuleLocation;//����λ��
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
                         "\"description\":\"����һ�����佺�ң������������İ�����������\"," +
                         "\"fail_reason\":\"\"," +
                         "\"obj_address\":{" +
                         "\"longitude\":116.4756," +
                         "\"latitude\":39.9058," +
                         "\"province\":\"������\"," +
                         "\"province_code\":\"110000\"," +
                         "\"city\":\"������\"," +
                         "\"city_code\":\"1101\"," +
                         "\"address_name\":\"��������\"," +
                         "\"address\":\"�����г���������·��26��\"," +
                         "\"address_tag\":\"���ز�;д��¥\"" +
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
            //��ʼ�����������
            if (InitArData())//�������OK
            {
                prefabSpawner.AsyCreateCapsule(capsule_data_info.ToString());
                //��ʼ�����������

            }
            //if (InitArData())//�������OK
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

            //        //���ɽ���
            //        prefabSpawner.AsyCreateCapsule(capsule_data_info.ToString());
            //    }
            //    else//������ݲ�OK
            //    {
            //        //��ѯ�û�λ����Ϣ
            //        Debug.Log("@@����500�ף��뷵��");
            //        GameObject tish00 = Instantiate(Resources.Load<GameObject>("Canvas"));
            //        tish00.transform.GetComponentInChildren<Text>().text = "����500�ף��뷵��";

            //    }
            //}
            //else
            //{
            //    Debug.Log("@@�����ݱ��գ����������û�λ��");
            //    GameObject tish00 = Instantiate(Resources.Load<GameObject>("Canvas"));
            //    tish00.transform.GetComponent<Text>().text = "�����ݱ��գ����������û�λ��";
            //    //��ѯ�û�λ����Ϣ
            //    locationRequestCoroutine = StartCoroutine(PeriodicLocationRequest());
            //}
        }

        /// <summary>
        /// ��ѯ�����û�λ��
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
            Debug.Log("�Ѵﵽ������������ֹͣ�����û�λ����Ϣ��");
        }
        /// <summary>
        /// �����û�����ʱ���õ���ѯֹͣ����
        /// </summary>
        private void StopLocationRequest()
        {
            if (locationRequestCoroutine != null)
            {
                StopCoroutine(locationRequestCoroutine);
                Debug.Log("�ѳɹ���ȡ�û�λ����Ϣ��ֹͣ����");
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
                JObject user_location = JObject.Parse(GameInfo.user_location);//�û�����json����
                Debug.Log(user_location.ToString());
                userLocation = new UserLocation();
                userLocation.longitude = double.Parse(user_location["longitude"].ToString());
                userLocation.latitude = double.Parse(user_location["latitude"].ToString());

            }
            else
            {
                Debug.Log("@user_locationΪ��");
                return false;
            }
            if (!string.IsNullOrEmpty(GameInfo.capsule_data))
                capsule_data_info = JObject.Parse(GameInfo.capsule_data);//�û�����json����
            else
            {
                Debug.Log("@capsule_dataΪ��");
                return false;
            }
            GameInfo.user_location = "";
            GameInfo.capsule_data = "";
            if (capsule_data_info != null)
            {
                if (capsule_data_info["obj_address"] != null)
                {
                    JObject obj_address = JObject.Parse(capsule_data_info["obj_address"].ToString());
                    Debug.Log("obj_address����" + obj_address.ToString());
                    capsuleLocation = new CapsuleLocation
                    {
                        longitude_capsule = double.Parse(obj_address["longitude"].ToString()),
                        latitude_capsule = double.Parse(obj_address["latitude"].ToString()),
                    };
                }
                else
                {
                    Debug.Log("@capsule_data+obj_addressΪ��");
                    return false;
                }
            }
            else
            {
                Debug.Log("@capsule_data_infoΪ��");
                return false;
            }
            return true;
        }

        public void RequestUserLocation()
        {
            //�������ɽ���
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.RequestUserLocation.ToString(),
                data = ""
            };
            string data_main = JsonUtility.ToJson(toNativeData);
            NativeBridge.Instance.SendMessageToNative(data_main);
            Debug.Log($"@�����û�λ����Ϣ: {data_main}");
        }

        /// <summary>
        /// ��֤AR��֤
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