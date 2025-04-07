using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace TimeStar.DigitalPlant
{
    public class DownloadHelper : MonoSingleton<DownloadHelper>
    {
        private static readonly HashSet<string> ongoingRequests = new HashSet<string>();

        public IEnumerator DownloadImageTexture(string url, System.Action<Texture2D> onSuccess, System.Action<string> onError, int maxRetries = 2, float timeout = 5f)
        {
            if (ongoingRequests.Contains(url))
            {
                onError?.Invoke("Request already in progress for this URL: " + url);
                yield break;
            }

            ongoingRequests.Add(url); // 标记为正在下载

            int attempt = 0;

            while (attempt < maxRetries)
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest(); // 等待请求完成

                    // 检查请求是否发生错误
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        // 记录错误并准备重试
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            onError?.Invoke(request.error + "@url:" + url); // 调用错误回调
                            ongoingRequests.Remove(url); // 移除标记
                            yield break; // 完成重试，退出协程
                        }
                        else
                        {
                            Debug.LogWarning("下载失败，正在重试... 尝试次数: " + attempt);
                        }
                    }
                    else
                    {
                        // 下载成功，将响应数据转换为 Texture2D
                        Texture2D texture = DownloadHandlerTexture.GetContent(request);
                        onSuccess?.Invoke(texture); // 调用成功回调
                        ongoingRequests.Remove(url); // 移除标记
                        yield break; // 成功，退出协程
                    }
                }
            }

            ongoingRequests.Remove(url); // 确保退出时移除标记
        }
        //// 下载图片并返回 Texture2D
        //public IEnumerator DownloadImageTexture(string url, System.Action<Texture2D> onSuccess, System.Action<string> onError)
        //{
        //    using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        //    {
        //        // 发送请求并等待响应
        //        yield return request.SendWebRequest();

        //        // 检查请求是否发生错误
        //        if (request.result != UnityWebRequest.Result.Success)
        //        {
        //            onError?.Invoke(request.error+"@url:"+ url); // 调用错误回调
        //        }
        //        else
        //        {
        //            // 将响应数据转换为 Texture2D
        //            Texture2D texture = DownloadHandlerTexture.GetContent(request);
        //            onSuccess?.Invoke(texture); // 调用成功回调
        //        }
        //    }
        //}

        // 测试下载图片
        private void DownloadTexture(string textureUrl)
        {
            StartCoroutine(DownloadImageTexture(textureUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        }

        // 下载成功的回调
        private void OnTextureDownloadSuccess(Texture2D texture)
        {
            // 这里可以做任何你想做的事情，比如显示图片
            Debug.Log("Image downloaded successfully.");
            // 例如，可以将纹理应用到一个对象上
            // GetComponent<Renderer>().material.mainTexture = texture;
        }

        // 下载失败的回调
        private void OnTextureDownloadError(string error)
        {
            Debug.LogError("Image download failed: " + error);
        }
    }
}