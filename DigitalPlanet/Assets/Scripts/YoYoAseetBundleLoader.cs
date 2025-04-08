using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class YoYoAseetBundleLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 初始化资源系统
        //YooAssets.Initialize();

        //// 创建默认的资源包
        //var package = YooAssets.CreatePackage("DefaultPackage");

        //////// 获取指定的资源包，如果没有找到会报错
        //////var package = YooAssets.GetPackage("DefaultPackage");

        ////// 获取指定的资源包，如果没有找到不会报错
        ////var package = YooAssets.TryGetPackage("DefaultPackage");

        //// 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        //YooAssets.SetDefaultPackage(package);
        StartCoroutine(AsyLoadScene());
    }

    public IEnumerator AsyLoadScene()
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        var package = YooAssets.CreatePackage("DefaultPackage");

        ////// 获取指定的资源包，如果没有找到会报错
        ////var package = YooAssets.GetPackage("DefaultPackage");

        //// 获取指定的资源包，如果没有找到不会报错
        //var package = YooAssets.TryGetPackage("DefaultPackage");

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);
        string location = "Assets/DigitalPlanet.AR/DigitalPlanetBundles/Prefabs/MainScene";
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive;
        var physicsMode = LocalPhysicsMode.None;
        bool suspendLoad = false;
        SceneHandle handle = package.LoadSceneAsync(location);
        yield return handle;
        Debug.Log($"Scene name is {handle.SceneName}");

    }
}
