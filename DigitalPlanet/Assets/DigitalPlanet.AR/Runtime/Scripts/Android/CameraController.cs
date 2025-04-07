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

            // ��ʼ��������
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true; // ����������
            }
        }

        private void StartCamera()
        {
            if (webcamTexture != null && webcamTexture.isPlaying) return;
            // ��ȡ���õ�����ͷ�豸
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length > 0)
            {
                webcamTexture = new WebCamTexture(devices[0].name);
                // rawImage.texture = webcamTexture;

                // ���� WebCamTexture ʱ��������ķֱ���
                //int requestedWidth = 1920; // Ҫ��Ŀ�ȣ�������Ϊ��λ
                //int requestedHeight = 1080; // Ҫ��ĸ߶ȣ�������Ϊ��λ
                //webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
                //rawImage.texture = webcamTexture;
                // ��������ͷ����
                webcamTexture.Play();

                // �������ͷ�Ƿ���ǰ������ͷ����Ҫ���о�����
                if (webcamTexture.videoVerticallyMirrored)
                {
                    // ��������Ϊ���Ϊ������1,0,1,1�����Ҳ෭ת (-1,0,-1,1)
                    rawImage.uvRect = new Rect(0, 0, 1, 1); // ������ʾ
                }
                else
                {
                    rawImage.uvRect = new Rect(0, 0, 1, -1); // ��ת Y ��
                }
            }
            else
            {
                Debug.LogError("û�п��õ�����ͷ");
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
            // �������λ�ú���ת���豸����������ͬ��
            if (SystemInfo.supportsGyroscope)
            {
                // λ�ã����Ա��ֲ�����߸����������
                // �������ת
                Quaternion deviceRotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(Input.gyro.attitude.eulerAngles);
                mainCamera.transform.rotation = deviceRotation;

            }
        }
        private void UpdateCameraPosition()
        {
            if (SystemInfo.supportsAccelerometer)
            {
                Vector3 acceleration = Input.acceleration;
                // ͨ�����ٶȸ������λ��
                mainCamera.transform.position += new Vector3(acceleration.x, 0, acceleration.y) * Time.deltaTime;
            }
        }
        void OnDestroy()
        {
            StopCamera();
        }

        private void StopCamera()
        {
            // ֹͣ����ͷ
            if (webcamTexture != null)
            {
                if (rawImage != null)
                    rawImage.texture = null;
                webcamTexture.Stop();
            }
        }
    }
}