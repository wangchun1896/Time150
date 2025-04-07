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
            if (GUILayout.Button("����"))
            {
                DownloadAssetBundle(abName);
            }
            if (GUILayout.Button("ж��"))
            {
                UnloadAndReleaseResources();
            }
        }
#endif
        // ���ݵ�ǰƽ̨��ȡ�ļ�������
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
        // ���� AssetBundle
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
            Debug.Log("ab·����" + uri);
#if UNITY_ANDROID
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("����AssetBundleʧ��: " + uwr.error);
            }
            else
            {
                loadedAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
                Debug.Log($"�ɹ�����AssetBundle: {loadedAssetBundle.name}");

                // ���������ʹ��loadedAssetBundle�����ؾ������Դ�����磺
                GameObject prefab = loadedAssetBundle.LoadAsset<GameObject>(ab_name);
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
                else
                {
                    Debug.Log($"δ�ܴ�AssetBundle�м�����Ϊ {ab_name} ��Ԥ����");
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
                    Debug.Log($"@@���سɹ�Loaded AssetBundle: {loadedAssetBundle.name}");
                    GameObject loadPrefab = loadedAssetBundle.LoadAsset<GameObject>(ab_name);
                    prefab = Instantiate(loadPrefab);
                }
                else
                {
                    Debug.LogError("@@@����ʧ��");
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("@�쳣@" + e.Message);
            }
#endif

            yield return null;
        }

        ///ж�ؼ��ص� AssetBundle 
        public void UnloadAndReleaseResources(Action callback = null)
        {
            // ����ʵ������ prefab��������ڣ�
            if (prefab != null)
            {
                DestroyImmediate(prefab);
                prefab = null; // ��� prefab ����
            }
            // ж�� AssetBundle
            if (loadedAssetBundle != null)
            {
                loadedAssetBundle.Unload(true); // ж�ز��ͷ����м��ص��ʲ�
                loadedAssetBundle = null; // �������
                Debug.Log("AssetBundle unloaded and memory released.");
            }
            UnloadUnusedResources(callback);
        }
        /// ж��δʹ�õ���Դ
        public void UnloadUnusedResources(Action callback = null)
        {
            StartCoroutine(UnloadUnusedResourcesCoroutine(callback));
        }
        private IEnumerator UnloadUnusedResourcesCoroutine(Action callback)
        {

            yield return new WaitForSeconds(0.2f);
            yield return Resources.UnloadUnusedAssets();
            // �ȴ�һ��ʱ��ȷ���첽�������
            yield return new WaitForSeconds(1f);
            System.GC.Collect();  // ���������������Խ�һ���ͷ��ڴ�
            if (GameInfo.IsDebug == "true")
                Debug.Log("��Դж�����.");
            callback?.Invoke();
        }


        //// ���ݵ�ǰƽ̨���� AssetBundle ������·��
        //private string GetAssetBundlePath(string assetBundleName)
        //{
        //    string platformFolderName = GetPlatformFolderName();
        //    string assetBundlePath = $"{Application.streamingAssetsPath}/AssetBundles/{platformFolderName}/{assetBundleName.ToLower()}"; // ȷ��������ļ���ǰ׺����ȷ��

        //    return assetBundlePath;
        //}
    }
}
