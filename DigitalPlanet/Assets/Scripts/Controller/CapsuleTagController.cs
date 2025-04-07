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
        // ��������ӵ������ʱҪִ�е��߼��������ӡ��Ϣ��ı���ɫ
        Debug.Log(gameObject.name + " @" + capsuleDetail);
        //GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
        //tish.GetComponentInChildren<Text>().text = capsuleDetail;
        //StartCoroutine(DestroyTish(tish));


        //��ʼ����������Native���ͳ�����Ϣ
        if (string.IsNullOrEmpty(capsuleDetail)) Debug.LogError("ʱ�ս�����ϸ��ϢΪ��");

        string common = CommandDataType.ShowTimeCapsule.ToString();//���ҵ��Ĭ�ϸ�native�������������
        if (transform.parent.name.Contains("����")) common = CommandDataType.ShowTimeCapsule_star.ToString();
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
