using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchAndInpuEffect3D : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.2f; // Ŀ��Ŵ���
    public float scaleSpeed = 2f; // �Ŵ�ͻ�ԭ���ٶ�
    public string forStarName;
    public bool isWheel = false;
    public StarWheelController wheelController;
    public XingZuoPanel xingZuoPanel;

    public UnityEvent OnSelected;
    public UnityEvent OnEnter;
    public UnityEvent OnCustomEvent;

    void Start()
    {
        originalScale = transform.localScale; // ����ԭʼ������ֵ
        //if(gameObject.name=="1")
        //{
        //    var methodName = OnSelected.GetPersistentMethodName(0);

        //    Debug.Log("--count--" + methodName);
        //}
    }

    void Update()
    {
#if !UNITY_EDITOR
        HandleTouchInput();
#elif UNITY_EDITOR
        HandleMouseInput(); // ���������봦��
#endif
    }

    private void HandleTouchInput()
    {
        // ��鴥������
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // ����ɨ������
                        if (hit.transform.gameObject == gameObject)
                        {
                             OnTouchBegin(); // �Ŵ�
                            if (isWheel)
                            {
                                wheelController.isPlayWheel = true;
                                wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                            }
                        }
                        else
                        {
                            OnTouchExit(); // ��С
                        }
                    }
                    break;
                case TouchPhase.Began:
                    Ray ray1 = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit1;
                    if (Physics.Raycast(ray1, out hit1))
                    {
                        // ����ɨ������
                        if (hit1.transform.gameObject == gameObject)
                        {
                            OnTouchBegin(); // �Ŵ�
                        }
                        else
                        {
                            OnTouchExit(); // ��С
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    Ray ray2 = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray2, out hit2))
                    {
                        // ����ɨ������
                        if (hit2.transform.gameObject == gameObject)
                        {
                            OnSelected?.Invoke(); // ѡ��
                            OnTouchExit(); // ��С
                            if (isWheel)
                            {
                                wheelController.isPlayWheel = false;
                                wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                            }

                        }
                        else
                        {
                            OnTouchExit(); // ��С
                            if (isWheel && wheelController.isPlayWheel && wheelController.lastSelectedTouchAndInputEffect3DGameobject != null &&
                                 wheelController.lastSelectedTouchAndInputEffect3DGameobject == this)
                            {
                                wheelController.lastSelectedTouchAndInputEffect3DGameobject.OnSelected?.Invoke();
                                Debug.Log(gameObject.name);
                            }
                        }
                    }
                    break;
            }
            
          

        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0)) // ����������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // �����е������Ƿ��Ǳ�����
                if (hit.transform.gameObject == gameObject)
                {
                    OnTouchBegin(); // ����� (�൱�ڻ���)
                    if (isWheel)
                    {
                        wheelController.isPlayWheel = true;
                        wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                    }
                }
                else
                {
                    OnTouchExit();
                }

            }
        }
        // ����������
        if (Input.GetMouseButtonDown(0)) // ����������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // �����е������Ƿ��Ǳ�����
                if (hit.transform.gameObject == gameObject)
                {
                    OnTouchBegin(); // ����� (�൱�ڻ���)
                }
              
            }
        }
        if (Input.GetMouseButtonUp(0)) // ����������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // �����е������Ƿ��Ǳ�����
                if (hit.transform.gameObject == gameObject)
                {
                    OnTouchExit();
                    OnSelected?.Invoke();
                    if (isWheel)
                    {
                        wheelController.isPlayWheel = false;
                        wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                    }
                }
                else
                {
                    OnTouchExit(); // ����뿪����
                    if (isWheel && wheelController.isPlayWheel&& wheelController.lastSelectedTouchAndInputEffect3DGameobject != null&&
                        wheelController.lastSelectedTouchAndInputEffect3DGameobject==this)
                    {
                        wheelController.lastSelectedTouchAndInputEffect3DGameobject.OnSelected?.Invoke();
                     //   Debug.Log(gameObject.name);
                    }
                       
                    //if (isWheel && wheelController.lastSelectedTouchAndInputEffect3DGameobject == null)
                    //    xingZuoPanel.XingZuoWheelImageAllClose();
                }
            }
        }
    }

    private void OnTouchBegin()
    {
        // �����ʼʱ���߼�
        ScaleObject(originalScale * scaleMultiplier);
        OnEnter?.Invoke();
    }
   
    private void OnTouchEnd()
    {
        // �������ʱ���߼�
        Debug.Log(gameObject.name + "ѡ��");
        ScaleObject(originalScale);
        //OnSelected?.Invoke();

    }
   
    private void OnTouchExit()
    {
        // ���������ѡ��ֱ�ӻ�ԭ���š�
        // ����Э���𽥻�ԭ
        ScaleObject(originalScale);
    }

    private void ScaleObject(Vector3 toScale)
    {
        transform.DOScale( toScale, scaleSpeed);
    }
}
