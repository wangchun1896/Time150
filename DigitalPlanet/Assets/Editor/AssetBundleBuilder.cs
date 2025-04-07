using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        // ָ��Ҫ����� Prefab �ļ���
        string prefabFolderPath = "Assets/Art/AssetBundlePrafebs"; // �滻Ϊ��� Prefab �ļ���·��
        string[] prefabPaths = Directory.GetFiles(prefabFolderPath, "*.prefab");

        // �������ͬƽ̨
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.StandaloneWindows, "PC");
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.Android, "Android");
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.iOS, "iOS");
    }

    private static void BuildAssetBundlesForPlatform(string[] prefabPaths, BuildTarget target, string platformFolderName)
    {
        // ���� AssetBundle �ļ���·��
        string assetBundleDir = $"{Application.streamingAssetsPath}/AssetBundles/{platformFolderName}/";
        if (!Directory.Exists(assetBundleDir))
        {
            Directory.CreateDirectory(assetBundleDir);
        }

        // ���֮ǰ�� AssetBundles
        string[] existingFiles = Directory.GetFiles(assetBundleDir);
        foreach (string file in existingFiles)
        {
            File.Delete(file);
        }

        // ���� AssetBundle ���Ʋ����
        foreach (string prefabPath in prefabPaths)
        {
            string assetName = Path.GetFileNameWithoutExtension(prefabPath);
            AssetImporter.GetAtPath(prefabPath).SetAssetBundleNameAndVariant(assetName.ToLower(), string.Empty); // ���� AssetBundle ����

            // ��� AssetBundles
            BuildPipeline.BuildAssetBundles(assetBundleDir, BuildAssetBundleOptions.None, target);
        }

        // ������
        Debug.Log($"AssetBundles for {platformFolderName} have been built successfully.");
    }
}
