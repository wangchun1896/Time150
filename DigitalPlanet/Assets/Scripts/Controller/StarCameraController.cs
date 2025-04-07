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
        // ������������
        if (Input.GetMouseButtonDown(0)&& m_camera!=null) // 0��ʾ���
        {
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // �������ߣ���ֻ����ignoreLayer�е�����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~clickIgnoreLayer))
            {
                if (hit.collider.gameObject.name.Contains("����Tag")) return;
                if (hit.collider.gameObject.GetComponent<CapsuleBev>() != null)
                {
                    // Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<CapsuleBev>().Clicked();
                }
                // �������ߵ����е�����
               // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 2f);
            }
            else
            {
                // ���û����ײ���������ߵ�Զ��
               // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 2f);
            }
        }
    }
}
