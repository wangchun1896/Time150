using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TimeStar.DigitalPlant
{
    [RequireComponent(typeof(Camera))]
    public class LayerSpecificCamera : MonoBehaviour
    {
        private Camera cam;
        private UniversalAdditionalCameraData cameraData;

        [SerializeField]
        private LayerMask targetLayer;

        [SerializeField]
        private bool useCustomClearFlags = true;

        [SerializeField]
        private CameraClearFlags customClearFlags = CameraClearFlags.Nothing;

        private void Awake()
        {
            // 获取相机组件
            cam = GetComponent<Camera>();

            // 获取URP相机数据组件
            cameraData = cam.GetUniversalAdditionalCameraData();
            if (cameraData == null)
            {
                cameraData = gameObject.AddComponent<UniversalAdditionalCameraData>();
            }

            // 设置相机
            SetupCamera();
        }

        private void SetupCamera()
        {
            // 设置只渲染指定层
            cam.cullingMask = targetLayer;

            // 设置清除标志
            if (useCustomClearFlags)
            {
                cam.clearFlags = customClearFlags;
            }
            else
            {
                cam.clearFlags = CameraClearFlags.Nothing;
            }

            // 设置为叠加相机
            cameraData.renderType = CameraRenderType.Overlay;

            // 如果这个相机是一个叠加相机，我们需要确保它在主相机的叠加列表中
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
                if (mainCameraData != null)
                {
                    // 检查是否已经在叠加列表中
                    bool alreadyInStack = false;
                    foreach (var cam in mainCameraData.cameraStack)
                    {
                        if (cam == this.cam)
                        {
                            alreadyInStack = true;
                            break;
                        }
                    }

                    // 如果不在叠加列表中，添加它
                    if (!alreadyInStack)
                    {
                        mainCameraData.cameraStack.Add(this.cam);
                    }
                }
            }
        }

        private void OnValidate()
        {
            // 如果在Inspector中修改了值，重新设置相机
            if (cam != null)
            {
                SetupCamera();
            }
        }

        private void OnDestroy()
        {
            // 清理：从主相机的堆栈中移除这个相机
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
                if (mainCameraData != null)
                {
                    mainCameraData.cameraStack.Remove(cam);
                }
            }
        }
    }
}