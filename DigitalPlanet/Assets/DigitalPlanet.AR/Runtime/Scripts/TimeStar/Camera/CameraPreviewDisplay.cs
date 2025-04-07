using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TimeStar.DigitalPlant
{
    public class CameraPreviewDisplay : MonoBehaviour
    {
        private WebCamTexture _webCamTexture;
        private RawImage _rawImage;
        private AspectRatioFitter _aspectRatioFitter;
        private Canvas _canvas;
        public UnityEngine.Camera CameraBG;
        
        void Start()
        {
            CreateCanvas();
            CreateRawImage();
            StartCoroutine(InitializeCamera());
        }

private IEnumerator InitializeCamera()
{
    Debug.Log("开始请求摄像头权限...");
    
    // 先检查当前权限状态
    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    {
        Debug.Log("已经具有摄像头权限，直接初始化");
        StartCamera();
        yield break;
    }

    // 请求摄像头权限
    yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    {
        Debug.Log("成功获取摄像头权限，开始初始化摄像头");
        StartCamera();
    }
    else
    {
        Debug.LogError("未获得摄像头权限，请检查系统权限设置");
    }
}
        void CreateCanvas()
        {
            GameObject canvasGo = new GameObject("CameraCanvas", typeof(Canvas));
            if (transform.parent != null) canvasGo.transform.SetParent(transform.parent);
            _canvas = canvasGo.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = CameraBG;
            canvasGo.layer = LayerMask.NameToLayer("ARMainCamera");
            CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            canvasGo.AddComponent<GraphicRaycaster>();
        }

        void CreateRawImage()
        {
            GameObject rawImageGo = new GameObject("CameraRawImage", typeof(RawImage));
            rawImageGo.transform.SetParent(_canvas.transform, false);
            _rawImage = rawImageGo.GetComponent<RawImage>();
            RectTransform rectTransform = _rawImage.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            _aspectRatioFitter = rawImageGo.AddComponent<AspectRatioFitter>();
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            rectTransform.localEulerAngles = new Vector3(0, 0, -90);
            rectTransform.localScale = Vector3.one * 2.3f;
        }

        void StartCamera()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                WebCamDevice device = WebCamTexture.devices[0];
                _webCamTexture = new WebCamTexture(device.name, 1920, 1080);
                _rawImage.texture = _webCamTexture;
                _webCamTexture.Play();
            }
            else
            {
                Debug.LogWarning("未找到摄像头设备。");
            }
        }

        void Update()
        {
            if (_webCamTexture == null || _webCamTexture.width <= 16 || _webCamTexture.height <= 16)
                return;

            if (_webCamTexture.videoVerticallyMirrored)
            {
                _rawImage.uvRect = new Rect(0, 1, 1, -1);
            }
            else
            {
                _rawImage.uvRect = new Rect(0, 0, 1, 1);
            }

            int texWidth = _webCamTexture.width;
            int texHeight = _webCamTexture.height;
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            float texAspect = (float)texWidth / texHeight;
            float screenAspect = (float)screenWidth / screenHeight;
            
            _aspectRatioFitter.aspectRatio = texAspect;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_canvas.transform);
        }

        void OnDestroy()
        {
            if (_webCamTexture != null && _webCamTexture.isPlaying)
            {
                _webCamTexture.Stop();
                
            }
            _webCamTexture = null;
        }

        private void OnApplicationQuit()
        {
            if (_webCamTexture != null && _webCamTexture.isPlaying)
            {
                _webCamTexture.Stop();

            }
            _webCamTexture = null;
        }
    }
}