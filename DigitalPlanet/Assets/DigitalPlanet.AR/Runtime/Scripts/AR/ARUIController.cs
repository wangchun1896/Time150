using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimeStar.DigitalPlant
{
    public class ARUIController : MonoBehaviour
    {
        public void OnClickBackButton()
        {
            // 通过NativeBridge发送消息
            //if (NativeBridge.Instance != null)
            //{
            //    GameInfo.ArReturn = true;
            //    SceneManager.LoadScene("Init");
            //}
            StartCoroutine(AsyOnBackButtonClick());
        }

        public IEnumerator AsyOnBackButtonClick()
        {
            yield return null;
            if (AssetBundleLoader.Instance != null)
            {
                //这里先执行卸载
                AssetBundleLoader.Instance.UnloadAndReleaseResources(() =>
                {
                    ReturenNativeData returenNative = new ReturenNativeData
                    {
                        scene_name = "ar"
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
        }

        public void OnBackButtonClick()
        {
            StartCoroutine(AsyOnBackButtonClick());

        }
    }
}