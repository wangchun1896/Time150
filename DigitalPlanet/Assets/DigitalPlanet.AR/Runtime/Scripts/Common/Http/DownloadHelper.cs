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

            ongoingRequests.Add(url); // ���Ϊ��������

            int attempt = 0;

            while (attempt < maxRetries)
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest(); // �ȴ��������

                    // ��������Ƿ�������
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        // ��¼����׼������
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            onError?.Invoke(request.error + "@url:" + url); // ���ô���ص�
                            ongoingRequests.Remove(url); // �Ƴ����
                            yield break; // ������ԣ��˳�Э��
                        }
                        else
                        {
                            Debug.LogWarning("����ʧ�ܣ���������... ���Դ���: " + attempt);
                        }
                    }
                    else
                    {
                        // ���سɹ�������Ӧ����ת��Ϊ Texture2D
                        Texture2D texture = DownloadHandlerTexture.GetContent(request);
                        onSuccess?.Invoke(texture); // ���óɹ��ص�
                        ongoingRequests.Remove(url); // �Ƴ����
                        yield break; // �ɹ����˳�Э��
                    }
                }
            }

            ongoingRequests.Remove(url); // ȷ���˳�ʱ�Ƴ����
        }
        //// ����ͼƬ������ Texture2D
        //public IEnumerator DownloadImageTexture(string url, System.Action<Texture2D> onSuccess, System.Action<string> onError)
        //{
        //    using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        //    {
        //        // �������󲢵ȴ���Ӧ
        //        yield return request.SendWebRequest();

        //        // ��������Ƿ�������
        //        if (request.result != UnityWebRequest.Result.Success)
        //        {
        //            onError?.Invoke(request.error+"@url:"+ url); // ���ô���ص�
        //        }
        //        else
        //        {
        //            // ����Ӧ����ת��Ϊ Texture2D
        //            Texture2D texture = DownloadHandlerTexture.GetContent(request);
        //            onSuccess?.Invoke(texture); // ���óɹ��ص�
        //        }
        //    }
        //}

        // ��������ͼƬ
        private void DownloadTexture(string textureUrl)
        {
            StartCoroutine(DownloadImageTexture(textureUrl, OnTextureDownloadSuccess, OnTextureDownloadError));
        }

        // ���سɹ��Ļص�
        private void OnTextureDownloadSuccess(Texture2D texture)
        {
            // ����������κ������������飬������ʾͼƬ
            Debug.Log("Image downloaded successfully.");
            // ���磬���Խ�����Ӧ�õ�һ��������
            // GetComponent<Renderer>().material.mainTexture = texture;
        }

        // ����ʧ�ܵĻص�
        private void OnTextureDownloadError(string error)
        {
            Debug.LogError("Image download failed: " + error);
        }
    }
}