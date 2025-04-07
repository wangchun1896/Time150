
using System.Collections;
using TimeStar.DigitalPlant;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimeStar.Bridge
{
    public class SceneLoadNotifier : MonoBehaviour
    {
        [SerializeField]
        private float notificationDelay = 1f; // 延迟时间，默认1秒

        private Coroutine timerCoroutine;
        private bool isTimerRunning;
       
        void Start()
        {
            StartCoroutine(NotifySceneLoadedAfterDelay());
        }
        private IEnumerator NotifySceneLoadedAfterDelay()
        {
            // 等待场景完全加载
            yield return new WaitForEndOfFrame();
            // 额外等待设定的延迟时间
            yield return new WaitForSeconds(0.5f);
#region 初次吊起unity,通知Native
            // 准备发送给Native的消息数据
            InitSceneToNativeData initSceneToNativeData = new InitSceneToNativeData
            {
                scene_Name = SceneManager.GetActiveScene().name
            };
            //初始化场景后向Native发送场景信息
            string notificationData=JsonUtility.ToJson(initSceneToNativeData);
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.initSceneLoaded.ToString(),
                data = notificationData
            };
            string data = JsonUtility.ToJson(toNativeData);

            // 通过NativeBridge发送消息
            if (NativeBridge.Instance != null)
            {
                NativeBridge.Instance.SendMessageToNative(data);
                //StartCoroutine(TimerCoroutine());////开始计时
                Debug.Log($"@场景加载完毕给native的消息: {data}");
            }
            else
            {
                Debug.LogWarning("NativeBridge instance not found!");
            }
            #endregion
            #region Main场景返回通知Native
            //if (GameInfo.MainReturn)//Main场景返回发送给native
            //{
            //    yield return Resources.UnloadUnusedAssets();
            //    System.GC.Collect();
            //    yield return new WaitForSeconds(notificationDelay);
            //    GameInfo.MainReturn = false;
            //    ReturenNativeData returenNative = new ReturenNativeData
            //    {
            //        scene_name = "main"
            //    };
            //    string s_returenNative = JsonUtility.ToJson(returenNative);
            //    ToNativeData toNativeData_main = new ToNativeData
            //    {
            //        command = CommandDataType.returnNative.ToString(),
            //        data = s_returenNative
            //    };
            //    string data_main = JsonUtility.ToJson(toNativeData_main);
            //    NativeBridge.Instance.SendMessageToNative(data_main);
            //    Debug.Log($"@返回按钮发送的数据: {data_main}");

            //    yield break;

            //}
            #endregion
            #region AR场景返回通知Native
            //if (GameInfo.ArReturn)//AR场景返回发送给native
            //{
            //    yield return Resources.UnloadUnusedAssets();
            //    System.GC.Collect();
            //    yield return new WaitForSeconds(notificationDelay);
            //    GameInfo.ArReturn = false;
            //    ReturenNativeData returenNative = new ReturenNativeData
            //    {
            //        scene_name = "ar"
            //    };
            //    string s_returenNative = JsonUtility.ToJson(returenNative);
            //    ToNativeData toNativeData_ar = new ToNativeData
            //    {
            //        command = CommandDataType.returnNative.ToString(),
            //        data = s_returenNative
            //    };
            //    string data_ar = JsonUtility.ToJson(toNativeData_ar);
            //    NativeBridge.Instance.SendMessageToNative(data_ar);
            //    Debug.Log($"@返回按钮发送的数据: {data_ar}");

            //    yield break;
            //}
            #endregion
        }
        // 启动计时器
        public void StartTimer()
        {
            if (!isTimerRunning)
            {
                isTimerRunning = true;
                timerCoroutine = StartCoroutine(TimerCoroutine());
            }
        }

        // 停止计时器
        public void StopTimer()
        {
            if (isTimerRunning && timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                isTimerRunning = false;
                Debug.Log("计时器已停止");
            }
        }

        // 计时协程
        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            if (isTimerRunning)
            {
                isTimerRunning = false;
                //返回Native
                if (AssetBundleLoader.Instance != null)
                {
                    Debug.Log("超时，准备返回native");
                    //这里先执行卸载
                    AssetBundleLoader.Instance.UnloadAndReleaseResources(() => {
                        // 切换到新场景
                        Debug.Log("超时，卸载无用资源，发送返回native消息");

                        ReturenNativeData returenNative = new ReturenNativeData
                        {
                            scene_name = "main"
                        };
                        string s_returenNative = JsonUtility.ToJson(returenNative);
                        ToNativeData toNativeData_main = new ToNativeData
                        {
                            command = CommandDataType.returnNative.ToString(),
                            data = s_returenNative
                        };
                        string data_main = JsonUtility.ToJson(toNativeData_main);
                        NativeBridge.Instance.SendMessageToNative(data_main);

                        Debug.Log($"@返回按钮发送的数据: {data_main}");
                    });
                }//返回
            }
        }



    }
}