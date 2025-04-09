using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TimeStar.DigitalPlant
{
    public class MainPanel : MonoBehaviour
    {
        public SpaceTimeTagPanel spaceTimeTagPanel;
        public void OnBackButtonClick()
        {
            StartCoroutine(AsyOnBackButtonClick());
        }
        public IEnumerator AsyOnBackButtonClick()
        {
            if (AssetBundleLoader.Instance.loadBG != null)
                AssetBundleLoader.Instance.loadBG.SetActive(true);
#if UNITY_IOS
        if (AssetBundleLoader.Instance != null)
        {
            //这里先执行卸载
            AssetBundleLoader.Instance.UnloadAndReleaseResources(() =>
            {
                // 切换到新场景
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
        }
#elif UNITY_ANDROID
            // 切换到新场景
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

#endif
            yield return null;
        }


        public void OnReturnUserReflashAll()
        {

#if UNITY_EDITOR
            GameInfo.tempTargetUser_Id = GameInfo.tempUser_Id;
#else
       GameInfo.Target_Id = GameInfo.User_Id;
#endif
            GameInfo.User_Id_TT = "";
            //主场景初始化
            if (GameManager.Instance != null)
            {
                GameManager.Instance.InitHttpData_main();
                ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfoRefresh_main_Dispatch_Index, "");
                //spaceTimeTagPanel.shiKongJiaoNangToggle.isOn = true;
                //spaceTimeTagPanel.shiKongJiaoNangToggle.onValueChanged.Invoke(spaceTimeTagPanel.shiKongJiaoNangToggle.isOn);
            }
        }

        public void OnMuskBuildsMarsClick()
        {
            // 通过NativeBridge发送消息
            //Debug.Log("OnMuskBuildsMarsClick");
            if (NativeBridge.Instance != null)
            {
                ToNativeData toNativeData = new ToNativeData
                {
                    command = CommandDataType.MuskBuildsMars.ToString(),
                    data = "",
                };
                string data = JsonUtility.ToJson(toNativeData);
                NativeBridge.Instance.SendMessageToNative(data);
            }
        }

        public void OnNotificationCenterClick()
        {
            // 通过NativeBridge发送消息
            if (NativeBridge.Instance != null)
            {
                ToNativeData toNativeData = new ToNativeData
                {
                    command = CommandDataType.NotificationCenter.ToString(),
                    data = "",
                };
                string data = JsonUtility.ToJson(toNativeData);
                NativeBridge.Instance.SendMessageToNative(data);
            }
        }



        public void OnPersonalLetterClick()
        {
            // 通过NativeBridge发送消息
            if (NativeBridge.Instance != null)
            {
                TTUserIdData tTUserIdData = new TTUserIdData
                {
                    tt_user_id = GameInfo.User_Id_TT,
                };
                string ttUserIdData = JsonUtility.ToJson(tTUserIdData);
#if UNITY_EDITOR
                ToNativeData toNativeData = new ToNativeData
                {
                    command = CommandDataType.PersonalLetter.ToString(),
                    data = ttUserIdData,
                };
#else
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.PersonalLetter.ToString(),
                data = ttUserIdData,
            };
#endif
                string data = JsonUtility.ToJson(toNativeData);
                NativeBridge.Instance.SendMessageToNative(data);
            }
        }


        public void ReleaseTimeCapsule_main()
        {
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.ReleaseTimeCapsule.ToString(),
                data = ""
            };
            string data = JsonUtility.ToJson(toNativeData);
            if (NativeBridge.Instance != null)
                NativeBridge.Instance.SendMessageToNative(data);
        }
        public void ReleaseTimeCapsule_star()
        {
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.ReleaseTimeCapsule_star.ToString(),
                data = ""
            };
            string data = JsonUtility.ToJson(toNativeData);
            if (NativeBridge.Instance != null)
                NativeBridge.Instance.SendMessageToNative(data);
        }
        public void ReleaseTimeStory_main()
        {
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.ReleaseTimeStory.ToString(),
                data = ""
            };
            string data = JsonUtility.ToJson(toNativeData);
            if (NativeBridge.Instance != null)
                NativeBridge.Instance.SendMessageToNative(data);
        }
    }
}