using System;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.Bridge
{
    public class NativeBridge : MonoBehaviour
    {
        public static NativeBridge Instance { get;  set; }

        public event Action<string> OnNativeMessageReceived;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                Debug.Log("NativeBridge initialized.");
            }
            //else
            //{
            //    Destroy(gameObject);
            //}
        }

#if UNITY_ANDROID
        private AndroidJavaObject androidActivity;
#endif
        /// <summary>
        /// unity发送消息给Android
        /// </summary>
        /// <param name="json"></param>
        public void CallAndroidFunction(string json)
        {
#if UNITY_ANDROID
            if (androidActivity == null)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    androidActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }

            androidActivity.Call("OnUnityCall", json);
            // androidActivity.Call("testOnUnityCall", json);
            Debug.LogWarning("androidActivity : " + androidActivity);
#else
        Debug.LogWarning("Attempted to call Android function on non-Android platform");
#endif
        }

#if UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CalliOSFunction(string json);
#else
        /// <summary>
        /// unity发送消息给ios
        /// </summary>
        /// <param name="json"></param>
        private static void CalliOSFunction(string json)
        {
            Debug.LogWarning("Attempted to call iOS function on non-iOS platform");
        }
#endif

        /// <summary>
        /// 接收Native发送来的消息
        /// </summary>
        /// <param name="json"></param>
        public void OnNativeCall(string json)
        {
            Debug.Log("@Unity接收到Native消息: " + json);
            OnNativeMessageReceived?.Invoke(json);
        }

        public void SendMessageToNative(string data)
        {
#if UNITY_ANDROID
            CallAndroidFunction(data);
#elif UNITY_IOS
        CalliOSFunction(data);
#else
        Debug.LogWarning("SendMessageToNative called on unsupported platform");
#endif
        }

       
    }
}
