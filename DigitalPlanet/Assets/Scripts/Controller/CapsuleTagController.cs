using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleTagController : MonoBehaviour
{
    public RawImage tex1;
    public RawImage tex2;
    public RawImage tex3;
    public TextMeshProUGUI text;
    public string capsuleDetail;
   
    private void OnEnable()
    {
        StartCoroutine(CloseSelf());
    }

    private IEnumerator CloseSelf()
    {
        yield return new WaitForSeconds(2);
        capsuleDetail = "";
        gameObject.SetActive(false);
    }

    public void OnTagClick()
    {
        // 在这里添加点击物体时要执行的逻辑，例如打印信息或改变颜色
        Debug.Log(gameObject.name + " @" + capsuleDetail);
        //GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
        //tish.GetComponentInChildren<Text>().text = capsuleDetail;
        //StartCoroutine(DestroyTish(tish));


        //初始化场景后向Native发送场景信息
        if (string.IsNullOrEmpty(capsuleDetail)) Debug.LogError("时空胶囊详细信息为空");

        string common = CommandDataType.ShowTimeCapsule.ToString();//胶囊点击默认给native发送主场景点击
        if (transform.parent.name.Contains("星座")) common = CommandDataType.ShowTimeCapsule_star.ToString();
#if !UNITY_EDITOR
        ToNativeData toNativeData_showTimeCapsule = new ToNativeData
        {
            command = common,
            data = capsuleDetail
        };
        string data = JsonUtility.ToJson(toNativeData_showTimeCapsule);
        NativeBridge.Instance.SendMessageToNative(data);
#endif
    }
    private IEnumerator DestroyTish(GameObject tish)
    {
        yield return new WaitForSeconds(1f);
        Destroy(tish);
    }
}
