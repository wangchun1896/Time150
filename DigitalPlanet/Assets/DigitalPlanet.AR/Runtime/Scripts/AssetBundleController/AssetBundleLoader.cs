using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;

namespace TimeStar.DigitalPlant
{
    public class AssetBundleLoader : MonoBehaviour
    {
        private AssetBundle loadedAssetBundle;
        public string abName = "mainscene";
        public GameObject prefab;

        public static AssetBundleLoader Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                Debug.Log("@AssetBundleLoader initialized.");
            }
            //else
            //{
            //    Destroy(gameObject);
            //}
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("加载"))
            {
                DownloadAssetBundle(abName);
            }
            if (GUILayout.Button("卸载"))
            {
                UnloadAndReleaseResources();
            }
        }
#endif
        // 根据当前平台获取文件夹名称
        private string GetPlatformFolderName()
        {
#if UNITY_ANDROID//Android
        return "Android";
#elif UNITY_IOS
        return "iOS";
#else
            return "PC";
#endif
        }
        // 下载 AssetBundle
        public void DownloadAssetBundle(string ab_name)
        {
            StartCoroutine(DownloadAssetBundleCoroutine(ab_name));
        }

        private IEnumerator DownloadAssetBundleCoroutine(string ab_name)
        {
            string uri = "";
            string platformFolderName = GetPlatformFolderName();
#if UNITY_ANDROID
        uri = "jar:file://" + Application.dataPath + "!/assets/" +"AssetBundles/"+ platformFolderName+"/" + ab_name;
        //uri = Application.streamingAssetsPath + "/AssetBundles/" + platformFolderName+"/" + ab_name;
#elif UNITY_IOS
        uri = Application.streamingAssetsPath + "/AssetBundles/" +  platformFolderName +"/" + ab_name;
#else
            uri = Application.streamingAssetsPath + "/AssetBundles/" + platformFolderName + "/" + ab_name;
#endif
            Debug.Log("ab路径：" + uri);
#if UNITY_ANDROID
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("加载AssetBundle失败: " + uwr.error);
            }
            else
            {
                loadedAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
                Debug.Log($"成功加载AssetBundle: {loadedAssetBundle.name}");

                // 在这里可以使用loadedAssetBundle来加载具体的资源，例如：
                GameObject prefab = loadedAssetBundle.LoadAsset<GameObject>(ab_name);
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
                else
                {
                    Debug.Log($"未能从AssetBundle中加载名为 {ab_name} 的预制体");
                }
            }
        }
#endif
#if UNITY_IOS || UNITY_EDITOR
            try
            {
                loadedAssetBundle = AssetBundle.LoadFromFile(uri);
                if (loadedAssetBundle != null)
                {
                    Debug.Log($"@@加载成功Loaded AssetBundle: {loadedAssetBundle.name}");
                    GameObject loadPrefab = loadedAssetBundle.LoadAsset<GameObject>(ab_name);
                    prefab = Instantiate(loadPrefab);
                }
                else
                {
                    Debug.LogError("@@@加载失败");
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("@异常@" + e.Message);
            }
#endif

            yield return null;
        }

        ///卸载加载的 AssetBundle 
        public void UnloadAndReleaseResources(Action callback = null)
        {
            // 销毁实例化的 prefab（如果存在）
            if (prefab != null)
            {
                DestroyImmediate(prefab);
                prefab = null; // 清空 prefab 引用
            }
            // 卸载 AssetBundle
            if (loadedAssetBundle != null)
            {
                loadedAssetBundle.Unload(true); // 卸载并释放所有加载的资产
                loadedAssetBundle = null; // 清空引用
                Debug.Log("AssetBundle unloaded and memory released.");
            }
            UnloadUnusedResources(callback);
        }
        /// 卸载未使用的资源
        public void UnloadUnusedResources(Action callback = null)
        {
            StartCoroutine(UnloadUnusedResourcesCoroutine(callback));
        }
        private IEnumerator UnloadUnusedResourcesCoroutine(Action callback)
        {

            yield return new WaitForSeconds(0.2f);
            yield return Resources.UnloadUnusedAssets();
            // 等待一段时间确保异步操作完成
            yield return new WaitForSeconds(1f);
            System.GC.Collect();  // 调用垃圾回收器以进一步释放内存
            if (GameInfo.IsDebug == "true")
                Debug.Log("资源卸载完成.");
            callback?.Invoke();
        }


        //// 根据当前平台生成 AssetBundle 的下载路径
        //private string GetAssetBundlePath(string assetBundleName)
        //{
        //    string platformFolderName = GetPlatformFolderName();
        //    string assetBundlePath = $"{Application.streamingAssetsPath}/AssetBundles/{platformFolderName}/{assetBundleName.ToLower()}"; // 确保这里的文件名前缀是正确的

        //    return assetBundlePath;
        //}
    }
}
