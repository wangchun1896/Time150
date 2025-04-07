using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class MainCameraController : MonoBehaviour
    {
        public LayerMask clickIgnoreLayer;
        private Camera m_camera;

        private void Start()
        {
            m_camera = GetComponent<Camera>();

        }
        private void OnDestroy()
        {
            m_camera = null;
        }

        private void Update()
        {
            // 检测鼠标左键点击
            if (Input.GetMouseButtonDown(0) && m_camera != null) // 0表示左键
            {
                //  Debug.Log("@Input.GetMouseButtonDown(0)管用");
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // 绘制射线，射线颜色为红色，持续时间为一帧
                //  Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
                // 发射射线，但只检测非ignoreLayer中的物体
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~clickIgnoreLayer))// 
                {
                    // Debug.Log("@Hit object: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.name.Contains("胶囊Tag")) return;
                    if (hit.collider.gameObject.GetComponent<CapsuleBev>() != null)
                    {
                        hit.collider.gameObject.GetComponent<CapsuleBev>().Clicked();
                    }

                }

            }


        }
    }
}