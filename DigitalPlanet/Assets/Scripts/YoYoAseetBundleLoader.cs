using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class YoYoAseetBundleLoader : MonoBehaviour
{
    private SceneHandle _sceneHandle;

    void Start()
    {
        StartCoroutine(LoadDigitalPlanetInitScene());
    }

    private IEnumerator LoadDigitalPlanetInitScene()
    {
        // ��ʼ�� YooAsset
        YooAssets.Initialize();
        // ����Ĭ�ϵ���Դ��
        var package = YooAssets.CreatePackage("DefaultPackage");
        // ���ø���Դ��ΪĬ�ϵ���Դ��������ʹ��YooAssets��ؼ��ؽӿڼ��ظ���Դ�����ݡ�
        YooAssets.SetDefaultPackage(package);
        OfflinePlayModeParameters initParameters = new();
       
//#if UNITY_ANDROID
//        string uri_UNITY_ANDROID = Application.streamingAssetsPath+"/yoo/DefaultPackage";
//        //uri = Application.streamingAssetsPath + "/yoo/" + DefaultPackage;
//        Debug.Log("@uri_UNITY_ANDROID:"+ uri_UNITY_ANDROID);
//        initParameters.BuildinRootDirectory= uri_UNITY_ANDROID;
//        Debug.Log("@initParameters.BuildinRootDirectory:" + uri_UNITY_ANDROID);

//#elif UNITY_IOS
//        string uri_UNITY_IOS = Application.streamingAssetsPath + "/yoo/DefaultPackage"  ;
//         initParameters.BuildinRootDirectory= uri_UNITY_IOS;
//#else
//#endif
        yield return package.InitializeAsync(initParameters);
       
        // ���� Init ����
        Debug.Log(initParameters.BuildinRootDirectory);
        try
        {
            _sceneHandle = YooAssets.LoadSceneAsync("Assets/DigitalPlanet.AR/DigitalPlanetBundles/Scenes/Init.unity", LoadSceneMode.Single);
        }                                                                           
        catch (System.Exception e)
        {

            Debug.Log("@@-----" + e.ToString());
        }
       
        yield return _sceneHandle;

        if (_sceneHandle.Status == EOperationStatus.Succeed)
        {
            Debug.Log("Init �������سɹ���");
        }
        else
        {
            Debug.LogError("Init ��������ʧ��: " + _sceneHandle.LastError);
        }
    }

    // ж�س���
    public void UnloadScene()
    {
        if (_sceneHandle.IsValid)
        {
            _sceneHandle.UnloadAsync();
            Debug.Log("Init ��������ж��...");
        }
    }

    private void OnDestroy()
    {
       // UnloadScene();
    }
}
