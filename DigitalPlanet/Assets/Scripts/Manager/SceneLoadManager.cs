using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    //public GameObject tiShi;
    //void OnEnable()
    //{
    //    // ע�᳡����������¼�
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnDisable()
    //{
    //    // ע��������������¼�
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //// �ص�����
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{

    //    // �������������Ҫ�ڳ������غ�ִ�е��߼�
    //    StartCoroutine(AsySceneLoadFinish());
    //}

    //private IEnumerator AsySceneLoadFinish()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //   // GameObject m_tishi= Instantiate(tiShi);
    //    if (!string.IsNullOrEmpty(GameInfo.Native_InitMessage))
    //    {
    //       // Debug.Log(GameInfo.Native_InitMessage);
    //    }
    //        //m_tishi.GetComponentInChildren<Text>().text = GameInfo.Native_InitMessage;
    //    else
    //    {
    //     //   Debug.Log(GameInfo.Native_InitMessage);
    //        //m_tishi.GetComponentInChildren<Text>().text = "GameInfo.Native_InitMessageû�ӵ�����";
    //    }

    //   // yield return new WaitForSeconds(2f);
    //   // Destroy(m_tishi);
    //}
}
