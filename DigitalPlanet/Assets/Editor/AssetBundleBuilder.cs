using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        // 指定要打包的 Prefab 文件夹
        string prefabFolderPath = "Assets/Art/AssetBundlePrafebs"; // 替换为你的 Prefab 文件夹路径
        string[] prefabPaths = Directory.GetFiles(prefabFolderPath, "*.prefab");

        // 打包到不同平台
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.StandaloneWindows, "PC");
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.Android, "Android");
        BuildAssetBundlesForPlatform(prefabPaths, BuildTarget.iOS, "iOS");
    }

    private static void BuildAssetBundlesForPlatform(string[] prefabPaths, BuildTarget target, string platformFolderName)
    {
        // 创建 AssetBundle 文件夹路径
        string assetBundleDir = $"{Application.streamingAssetsPath}/AssetBundles/{platformFolderName}/";
        if (!Directory.Exists(assetBundleDir))
        {
            Directory.CreateDirectory(assetBundleDir);
        }

        // 清空之前的 AssetBundles
        string[] existingFiles = Directory.GetFiles(assetBundleDir);
        foreach (string file in existingFiles)
        {
            File.Delete(file);
        }

        // 设置 AssetBundle 名称并打包
        foreach (string prefabPath in prefabPaths)
        {
            string assetName = Path.GetFileNameWithoutExtension(prefabPath);
            AssetImporter.GetAtPath(prefabPath).SetAssetBundleNameAndVariant(assetName.ToLower(), string.Empty); // 设置 AssetBundle 名称

            // 打包 AssetBundles
            BuildPipeline.BuildAssetBundles(assetBundleDir, BuildAssetBundleOptions.None, target);
        }

        // 输出结果
        Debug.Log($"AssetBundles for {platformFolderName} have been built successfully.");
    }
}
