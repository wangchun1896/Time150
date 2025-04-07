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
            // 在这里添加点击物体时要执行的逻辑，例如打印信息或改变颜色
            Debug.Log(gameObject.name + " @" + capsleDetal);
            //#if UNITY_EDITOR
            //        GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
            //        tish.GetComponentInChildren<Text>().text = capsleDetal;
            //#endif


            //初始化场景后向Native发送场景信息
            if (string.IsNullOrEmpty(capsleDetal)) Debug.LogError("时空胶囊详细信息为空");
            string common = CommandDataType.ShowTimeCapsule.ToString();//胶囊点击默认给native发送主场景点击
            if (transform.parent.name.Contains("星座")) common = CommandDataType.ShowTimeCapsule_star.ToString();
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