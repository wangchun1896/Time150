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
            // ������������
            if (Input.GetMouseButtonDown(0) && m_camera != null) // 0��ʾ���
            {
                //  Debug.Log("@Input.GetMouseButtonDown(0)����");
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // �������ߣ�������ɫΪ��ɫ������ʱ��Ϊһ֡
                //  Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
                // �������ߣ���ֻ����ignoreLayer�е�����
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~clickIgnoreLayer))// 
                {
                    // Debug.Log("@Hit object: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.name.Contains("����Tag")) return;
                    if (hit.collider.gameObject.GetComponent<CapsuleBev>() != null)
                    {
                        hit.collider.gameObject.GetComponent<CapsuleBev>().Clicked();
                    }

                }

            }


        }
    }
}