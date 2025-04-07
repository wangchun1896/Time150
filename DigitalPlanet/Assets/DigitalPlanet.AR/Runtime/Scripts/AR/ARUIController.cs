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
            // ͨ��NativeBridge������Ϣ
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
                //������ִ��ж��
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
                    Debug.Log($"@���ذ�ť���͵�����: {data_main}");

                });
            }
        }

        public void OnBackButtonClick()
        {
            StartCoroutine(AsyOnBackButtonClick());

        }
    }
}