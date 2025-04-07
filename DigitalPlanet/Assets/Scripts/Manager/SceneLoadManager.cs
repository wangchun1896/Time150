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
    //    // 注册场景加载完成事件
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnDisable()
    //{
    //    // 注销场景加载完成事件
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //// 回调方法
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{

    //    // 在这里添加你需要在场景加载后执行的逻辑
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
    //        //m_tishi.GetComponentInChildren<Text>().text = "GameInfo.Native_InitMessage没接到数据";
    //    }

    //   // yield return new WaitForSeconds(2f);
    //   // Destroy(m_tishi);
    //}
}
