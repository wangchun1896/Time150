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
        // ��ʼ����Դϵͳ
        //YooAssets.Initialize();

        //// ����Ĭ�ϵ���Դ��
        //var package = YooAssets.CreatePackage("DefaultPackage");

        //////// ��ȡָ������Դ�������û���ҵ��ᱨ��
        //////var package = YooAssets.GetPackage("DefaultPackage");

        ////// ��ȡָ������Դ�������û���ҵ����ᱨ��
        ////var package = YooAssets.TryGetPackage("DefaultPackage");

        //// ���ø���Դ��ΪĬ�ϵ���Դ��������ʹ��YooAssets��ؼ��ؽӿڼ��ظ���Դ�����ݡ�
        //YooAssets.SetDefaultPackage(package);
        StartCoroutine(AsyLoadScene());
    }

    public IEnumerator AsyLoadScene()
    {
        // ��ʼ����Դϵͳ
        YooAssets.Initialize();

        // ����Ĭ�ϵ���Դ��
        var package = YooAssets.CreatePackage("DefaultPackage");

        ////// ��ȡָ������Դ�������û���ҵ��ᱨ��
        ////var package = YooAssets.GetPackage("DefaultPackage");

        //// ��ȡָ������Դ�������û���ҵ����ᱨ��
        //var package = YooAssets.TryGetPackage("DefaultPackage");

        // ���ø���Դ��ΪĬ�ϵ���Դ��������ʹ��YooAssets��ؼ��ؽӿڼ��ظ���Դ�����ݡ�
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
