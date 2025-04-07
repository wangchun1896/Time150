using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class CapsuleBev : MonoBehaviour
    {
        [TextArea]
        public string capsleDetal;
        JObject capsuleInfo;
        private void OnEnable()
        {

        }
        private void Start()
        {
            if (string.IsNullOrEmpty(capsleDetal)) return;
            capsuleInfo = JObject.Parse(capsleDetal);

            gameObject.name = capsuleInfo["description"].ToString();
        }

        private void OnDestroy()
        {
            if (capsuleInfo != null)
                capsuleInfo = null;
        }
        public void Clicked()
        {
            // ��������ӵ������ʱҪִ�е��߼��������ӡ��Ϣ��ı���ɫ
            Debug.Log(gameObject.name + " @" + capsleDetal);
            //#if UNITY_EDITOR
            //        GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
            //        tish.GetComponentInChildren<Text>().text = capsleDetal;
            //#endif


            //��ʼ����������Native���ͳ�����Ϣ
            if (string.IsNullOrEmpty(capsleDetal)) Debug.LogError("ʱ�ս�����ϸ��ϢΪ��");
            string common = CommandDataType.ShowTimeCapsule.ToString();//���ҵ��Ĭ�ϸ�native�������������
            if (transform.parent.name.Contains("����")) common = CommandDataType.ShowTimeCapsule_star.ToString();
#if !UNITY_EDITOR
        ToNativeData toNativeData_showTimeCapsule = new ToNativeData
        {
           
            command = common,
            data = capsleDetal
        };
        string data = JsonUtility.ToJson(toNativeData_showTimeCapsule);
        NativeBridge.Instance.SendMessageToNative(data);
#endif
        }



        private IEnumerator DestroyTish(GameObject tish)
        {
            yield return new WaitForSeconds(2f);
            Destroy(tish);
        }
    }
}