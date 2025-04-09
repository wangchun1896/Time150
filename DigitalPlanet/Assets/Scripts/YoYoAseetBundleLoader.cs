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
        // 初始化 YooAsset
        YooAssets.Initialize();
        // 创建默认的资源包
        var package = YooAssets.CreatePackage("DefaultPackage");
        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
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
       
        // 加载 Init 场景
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
            Debug.Log("Init 场景加载成功！");
        }
        else
        {
            Debug.LogError("Init 场景加载失败: " + _sceneHandle.LastError);
        }
    }

    // 卸载场景
    public void UnloadScene()
    {
        if (_sceneHandle.IsValid)
        {
            _sceneHandle.UnloadAsync();
            Debug.Log("Init 场景正在卸载...");
        }
    }

    private void OnDestroy()
    {
       // UnloadScene();
    }
}
