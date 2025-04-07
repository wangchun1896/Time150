using UnityEngine;
using UnityEngine.UI;

namespace TimeStar.DigitalPlant
{
    public class CameraController : MonoBehaviour
    {
        private WebCamTexture webcamTexture;
        public RawImage rawImage;
        private Camera mainCamera;
        void Start()
        {
            Application.targetFrameRate = 60;
            mainCamera = Camera.main;
            StartCamera();

            // 初始化陀螺仪
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true; // 启用陀螺仪
            }
        }

        private void StartCamera()
        {
            if (webcamTexture != null && webcamTexture.isPlaying) return;
            // 获取可用的摄像头设备
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length > 0)
            {
                webcamTexture = new WebCamTexture(devices[0].name);
                // rawImage.texture = webcamTexture;

                // 创建 WebCamTexture 时设置所需的分辨率
                //int requestedWidth = 1920; // 要求的宽度，以像素为单位
                //int requestedHeight = 1080; // 要求的高度，以像素为单位
                //webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
                //rawImage.texture = webcamTexture;
                // 播放摄像头画面
                webcamTexture.Play();

                // 检查摄像头是否是前置摄像头，需要进行镜像处理
                if (webcamTexture.videoVerticallyMirrored)
                {
                    // 这里设置为左侧为正常（1,0,1,1），右侧翻转 (-1,0,-1,1)
                    rawImage.uvRect = new Rect(0, 0, 1, 1); // 正常显示
                }
                else
                {
                    rawImage.uvRect = new Rect(0, 0, 1, -1); // 翻转 Y 轴
                }
            }
            else
            {
                Debug.LogError("没有可用的摄像头");
            }
        }

        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.O))
            //{
            //    StartCamera();
            //}
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    StopCamera();
            //}
#if !UNITY_EDITOR
        UpdateCameraPosition();
        UpdateCameraPositionAndRotation();
#endif
        }
        private void UpdateCameraPositionAndRotation()
        {
            // 将相机的位置和旋转与设备的陀螺仪相同步
            if (SystemInfo.supportsGyroscope)
            {
                // 位置：可以保持不变或者根据需求调整
                // 相机的旋转
                Quaternion deviceRotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(Input.gyro.attitude.eulerAngles);
                mainCamera.transform.rotation = deviceRotation;

            }
        }
        private void UpdateCameraPosition()
        {
            if (SystemInfo.supportsAccelerometer)
            {
                Vector3 acceleration = Input.acceleration;
                // 通过加速度更新相机位置
                mainCamera.transform.position += new Vector3(acceleration.x, 0, acceleration.y) * Time.deltaTime;
            }
        }
        void OnDestroy()
        {
            StopCamera();
        }

        private void StopCamera()
        {
            // 停止摄像头
            if (webcamTexture != null)
            {
                if (rawImage != null)
                    rawImage.texture = null;
                webcamTexture.Stop();
            }
        }
    }
}