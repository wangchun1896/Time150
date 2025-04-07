using Newtonsoft.Json.Linq;
using System;
using TimeStar.DigitalPlant;
using TimeStar.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace TimeStar.Bridge
{
    public class NativeMessageHandler : MonoBehaviour
    {
        private static NativeMessageHandler Instance { get; set; }
        public SceneLoadNotifier sceneLoadNotifier;
#if UNITY_EDITOR
        private void OnGUI()
        {
            if(GUILayout.Button("asdlfa埃里克森的分厘卡即使对方"))
            {
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
                       "\"longitude\":116.46711730957031," +
                       "\"latitude\":39.95938491821289," +
                       "\"province\":\"北京市\"," +
                       "\"province_code\":\"110000\"," +
                       "\"city\":\"北京市\"," +
                       "\"city_code\":\"1101\"," +
                       "\"address_name\":\"海航大厦\"," +
                       "\"address\":\"北京市朝阳区霄云路甲26号\"," +
                       "\"address_tag\":\"房地产;写字楼\"" +
                       "}}";
                GameInfo.capsule_data =ss;
                GameInfo.user_location = "{\"longitude\":\"116.46711730957031\",\"latitude\":\"39.95938491821289\"}";
                if (GameObject.Find("CanvasInit") != null)
                    GameObject.Find("CanvasInit").SetActive(false);
                AssetBundleLoader.Instance.DownloadAssetBundle("arscene");
            }
        }
#endif
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                Debug.Log("NativeMessageHandler initialized.");
            }
            //else
            //{
            //    Destroy(gameObject);
            //}
        }
        private void Start()
        {
            NativeBridge.Instance.OnNativeMessageReceived += HandleNativeMessage;
        }
        private void OnDestroy()
        {
            if (NativeBridge.Instance != null)
            {
                NativeBridge.Instance.OnNativeMessageReceived -= HandleNativeMessage;
            }
        }
        private void HandleNativeMessage(string json)
        {
            Debug.Log("@Unity开始处理Native接到的消息" + json);
            try
            {
                JObject my_message = JObject.Parse(json);//提取命令
                switch (my_message["command"].ToString())//根据命令类型进行判断
                {
                    case "spawn":
                        NativeMessage message = JsonUtility.FromJson<NativeMessage>(my_message["data"].ToString());
                        Vector3 position = new Vector3(
                        message.position.x,
                        message.position.y,
                        message.position.z
                    );
                        Vector3 rotation = new Vector3(
                            message.rotation.x,
                            message.rotation.y,
                            message.rotation.z
                        );
                        float modelType = message.modelType;
                        StartCoroutine(
                            PrefabSpawner.Instance.SpawnPrefabWithDelay(position, rotation, modelType, message.id, json));
                        break;
                    case "destroy":
                        NativeMessage messagedestroy = JsonUtility.FromJson<NativeMessage>(my_message["data"].ToString());
                        PrefabSpawner.Instance.DestroyPrefab(messagedestroy.id);
                        break;
                    case "destroy_batch":
                        NativeMessage messagedestroy_batch = JsonUtility.FromJson<NativeMessage>(my_message["data"].ToString());
                        PrefabSpawner.Instance.DestroyPrefabs(messagedestroy_batch.ids);
                        break;
                    case "destroy_all":
                        PrefabSpawner.Instance.DestroyAllPrefabs();
                        break;
                    case GameInfo.DestroyTimeCapsuleReturn:
                    case GameInfo.ReleaseTimeCapsuleReturn:
                        //GameObject tish0 = Instantiate(Resources.Load<GameObject>("Canvas"));
                        //tish0.GetComponentInChildren<Text>().text = "收到胶囊刷新消息"+ my_message["command"].ToString();
                        if(GameInfo.IsDebug=="true")
                        Debug.Log("收到胶囊刷新消息" + my_message["command"].ToString());
                        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, "");
                        break;
                    case GameInfo.DestroyTimeCapsuleReturn_star:
                    case GameInfo.ReleaseTimeCapsuleReturn_star:
                        //GameObject tish00 = Instantiate(Resources.Load<GameObject>("Canvas"));
                        //tish00.GetComponentInChildren<Text>().text = "收到星座胶囊刷新消息" + my_message["command"].ToString();
                        if (GameInfo.IsDebug == "true")
                            Debug.Log("收到星座胶囊刷新消息" + my_message["command"].ToString());
                        if (GameManager.Instance != null)
                            GameManager.Instance.InitHttpData_star();
                        break;
                    case GameInfo.DestroyTimeStoryReturn:
                    case GameInfo.ReleaseTimeStoryReturn:
                        //GameObject tish1 = Instantiate(Resources.Load<GameObject>("Canvas"));
                        //tish1.GetComponentInChildren<Text>().text = "收到故事刷新消息" + my_message["command"].ToString();
                        if (GameInfo.IsDebug == "true")
                            Debug.Log("收到故事刷新消息" + my_message["command"].ToString());
                        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeStoryInfoRefresh_main_Dispatch_Index, "");

                        //派发事件 //InitHttpData_star
                        break;
                    case GameInfo.CreateUserBirthdayReturn:
                        //GameObject tish2 = Instantiate(Resources.Load<GameObject>("Canvas"));
                        //tish2.GetComponentInChildren<Text>().text = "收到星座生日刷新消息" + my_message["command"].ToString();
                        if (GameInfo.IsDebug == "true")
                            Debug.Log("收到星座生日刷新消息" + my_message["command"].ToString());
                        if(GameManager.Instance!=null)
                            GameManager.Instance.InitHttpData_star();//初始化用户数据
                        break;
                    case GameInfo.NoCreateUserBirthdayReturn:
                        //GameObject tish3 = Instantiate(Resources.Load<GameObject>("Canvas"));
                        //tish3.GetComponentInChildren<Text>().text = "收到星座生日刷新消息" + my_message["command"].ToString();
                        if (GameInfo.IsDebug == "true")
                            Debug.Log("收到星座生日刷新消息" + my_message["command"].ToString());
                        //返回主页面
                        UIManager.Instance.starReturnMainButton.GetComponent<Button>().onClick.Invoke();
                        break;
                    case GameInfo.ReturnFromNativeToTargetMain://从native返回去目标主场景
                        TargetSceneData returnFromNativeTomainData = JsonUtility.FromJson<TargetSceneData>(my_message["data"].ToString());
                        //从native拿到Target_id
                        GameInfo.Target_Id = returnFromNativeTomainData.target_id;
                        //切换到主场景
                        if(UIManager.Instance!=null)
                        {
                            UIManager.Instance.StarToMain_target();
                        }
                        //主场景初始化
                        if(GameManager.Instance!=null)
                        {
                            GameManager.Instance.InitHttpData_main();
                            ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, "");
                        }
                       
                        break;
                    case GameInfo.ReturnFromMainToNative://从main返回到native
                        UIManager.Instance.FindChildComponents<MainPanel>()[0].OnBackButtonClick();
                        break;
                    case GameInfo.ReturnFromARToNative://从main返回到native
                        FindObjectOfType<ARUIController>().OnBackButtonClick();
                        break;
                    case GameInfo.loadScene://加载场景
                        sceneLoadNotifier.StopTimer();
                        LoadSceneData loadSceneData = JsonUtility.FromJson<LoadSceneData>(my_message["data"].ToString());
                        //从native拿到id和token
                        GameInfo.User_Id = loadSceneData.user_id;
                        GameInfo.Target_Id = loadSceneData.user_id;
                        GameInfo.User_Token = loadSceneData.user_token;
                        GameInfo.IsDebug = loadSceneData.isDebug;
                        //向Http获取用户数据

                        switch (loadSceneData.scene_name)
                        {
                            case "main":
                                GameInfo.Native_InitMessage = my_message["data"].ToString();
                                RequestData tempData_main = JsonUtility.FromJson<RequestData>(loadSceneData.user_token);
                                GameInfo.tokenData = tempData_main;
                                GameInfo.app_type = tempData_main.app_type;
                                GameInfo.device = tempData_main.device;
                                GameInfo.device_id = tempData_main.device_id;
                                GameInfo.authorization = tempData_main.authorization;
                                GameInfo.user_agent = tempData_main.user_agent;
                                GameInfo.accept_encoding = tempData_main.accept_encoding;
                                GameInfo.login_type = tempData_main.login_type;
                                GameInfo.timezone = tempData_main.timezone;
                                GameInfo.lang = tempData_main.lang;
                                GameInfo.timestamp = tempData_main.timestamp;
                                GameInfo.version = tempData_main.version;
                                UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.User_Id, target_user_id = GameInfo.User_Id };
                                string interfaceParam= JsonUtility.ToJson(userInfoData);
                                string httpData= HttpHelper.HttpRequest(GameInfo.userInfoInterface, interfaceParam);
                                if (httpData.Contains("NetworkError"))
                                {
                                   
                                    if (GameInfo.IsDebug == "true")
                                        Debug.Log("没有用户数据:"+ httpData);
                                }
                                else
                                {
                                    //跳转场景
                                    if (GameObject.Find("CanvasInit")!=null)
                                        GameObject.Find("CanvasInit").SetActive(false);
                                    AssetBundleLoader.Instance.DownloadAssetBundle("mainscene");
                                }
                                break;
                            case "ar":
                                GameInfo.Native_InitMessage = my_message["data"].ToString();
                                RequestData tempData_ar= JsonUtility.FromJson<RequestData>(loadSceneData.user_token);
                                GameInfo.tokenData = tempData_ar;
                                GameInfo.app_type = tempData_ar.app_type;
                                GameInfo.device = tempData_ar.device;
                                GameInfo.device_id = tempData_ar.device_id;
                                GameInfo.authorization = tempData_ar.authorization;
                                GameInfo.user_agent = tempData_ar.user_agent;
                                GameInfo.accept_encoding = tempData_ar.accept_encoding;
                                GameInfo.login_type = tempData_ar.login_type;
                                GameInfo.timezone = tempData_ar.timezone;
                                GameInfo.lang = tempData_ar.lang;
                                GameInfo.timestamp = tempData_ar.timestamp;
                                GameInfo.version = tempData_ar.version;

                                UserInfoData userInfoData_ar = new UserInfoData { user_id = GameInfo.User_Id, target_user_id = GameInfo.User_Id };
                                string interfaceParam_ar = JsonUtility.ToJson(userInfoData_ar);
                                string httpData_ar = HttpHelper.HttpRequest(GameInfo.userInfoInterface, interfaceParam_ar);
                                if (httpData_ar.Contains("NetworkError"))
                                {
                                    if (GameInfo.IsDebug == "true")
                                        Debug.Log("NetworkError"+ httpData_ar);
                                }
                                else
                                {
                                    GameInfo.capsule_data = loadSceneData.capsule_data;
                                    GameInfo.user_location = loadSceneData.user_location;
                                    Debug.Log("@@capsule_data:" + GameInfo.capsule_data);
                                    Debug.Log("@@user_location:" + GameInfo.user_location);

                                    if (GameObject.Find("CanvasInit") != null)
                                        GameObject.Find("CanvasInit").SetActive(false);
                                    AssetBundleLoader.Instance.DownloadAssetBundle("arscene");
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    
                    default:
                        if (GameInfo.IsDebug == "true")
                            Debug.LogWarning("未知 command: " + my_message["command"].ToString());
                        break;
                }
            }
            catch (Exception e)
            {
                if (GameInfo.IsDebug == "true")
                    Debug.LogError("Error parsing JSON: " + e.Message);
            }
        }


       
        private void OnMainSceneLoaded()
        {
            // 这里写加载完成后的逻辑
            Debug.Log("Main加载完毕,开始加载场景资源!");
            // 加载AB资源
            AssetBundleLoader.Instance.DownloadAssetBundle("mainscene");
        }
        private void OnArSceneLoaded()
        {
            // 这里写加载完成后的逻辑
            Debug.Log("AR加载完毕,开始加载场景资源!");
            // 加载AB资源
            AssetBundleLoader.Instance.DownloadAssetBundle("arscene");
        }
    }
}