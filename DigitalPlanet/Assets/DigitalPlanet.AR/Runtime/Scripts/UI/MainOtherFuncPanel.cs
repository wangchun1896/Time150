using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class MainOtherFuncPanel : MonoBehaviour
    {
        public void OnEnjoyHomeClick()
        {
            // ͨ��NativeBridge������Ϣ
            if (NativeBridge.Instance != null)
            {
                IdData idData = new IdData();
                if (!GameManager.Instance.isTarget)//�Լ�
                    idData.userid = GameInfo.User_Id;
                else
                    idData.userid = GameInfo.Target_Id;

                string id = JsonUtility.ToJson(idData);
                ToNativeData toNativeData = new ToNativeData
                {
                    command = CommandDataType.EnjoyHome.ToString(),
                    data = id,
                };
                string data = JsonUtility.ToJson(toNativeData);
                NativeBridge.Instance.SendMessageToNative(data);
            }
        }
    }
}