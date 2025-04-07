using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCameraController : MonoBehaviour
{
    public LayerMask clickIgnoreLayer;
    private Camera m_camera;
    private void Awake()
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
        if (Input.GetMouseButtonDown(0)&& m_camera!=null) // 0表示左键
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 发射射线，但只检测非ignoreLayer中的物体
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~clickIgnoreLayer))
            {
                if (hit.collider.gameObject.name.Contains("胶囊Tag")) return;
                if (hit.collider.gameObject.GetComponent<CapsuleBev>() != null)
                {
                    // Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<CapsuleBev>().Clicked();
                }
                // 绘制射线到命中的物体
               // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 2f);
            }
            else
            {
                // 如果没有碰撞，绘制射线到远方
               // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
            }
        }
    }
}
