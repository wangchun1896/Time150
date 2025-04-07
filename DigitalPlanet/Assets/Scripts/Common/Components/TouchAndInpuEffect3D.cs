using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchAndInpuEffect3D : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.2f; // 目标放大倍数
    public float scaleSpeed = 2f; // 放大和还原的速度
    public string forStarName;
    public bool isWheel = false;
    public StarWheelController wheelController;
    public XingZuoPanel xingZuoPanel;

    public UnityEvent OnSelected;
    public UnityEvent OnEnter;
    public UnityEvent OnCustomEvent;

    void Start()
    {
        originalScale = transform.localScale; // 保存原始的缩放值
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
        HandleMouseInput(); // 添加鼠标输入处理
#endif
    }

    private void HandleTouchInput()
    {
        // 检查触控输入
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
                        // 滑动扫到对象
                        if (hit.transform.gameObject == gameObject)
                        {
                             OnTouchBegin(); // 放大
                            if (isWheel)
                            {
                                wheelController.isPlayWheel = true;
                                wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                            }
                        }
                        else
                        {
                            OnTouchExit(); // 缩小
                        }
                    }
                    break;
                case TouchPhase.Began:
                    Ray ray1 = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit1;
                    if (Physics.Raycast(ray1, out hit1))
                    {
                        // 滑动扫到对象
                        if (hit1.transform.gameObject == gameObject)
                        {
                            OnTouchBegin(); // 放大
                        }
                        else
                        {
                            OnTouchExit(); // 缩小
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    Ray ray2 = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray2, out hit2))
                    {
                        // 滑动扫到对象
                        if (hit2.transform.gameObject == gameObject)
                        {
                            OnSelected?.Invoke(); // 选定
                            OnTouchExit(); // 缩小
                            if (isWheel)
                            {
                                wheelController.isPlayWheel = false;
                                wheelController.lastSelectedTouchAndInputEffect3DGameobject = this;
                            }

                        }
                        else
                        {
                            OnTouchExit(); // 缩小
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
        if (Input.GetMouseButton(0)) // 鼠标左键按下
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 检查击中的物体是否是本对象
                if (hit.transform.gameObject == gameObject)
                {
                    OnTouchBegin(); // 鼠标点击 (相当于滑入)
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
        // 检查鼠标输入
        if (Input.GetMouseButtonDown(0)) // 鼠标左键按下
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 检查击中的物体是否是本对象
                if (hit.transform.gameObject == gameObject)
                {
                    OnTouchBegin(); // 鼠标点击 (相当于滑入)
                }
              
            }
        }
        if (Input.GetMouseButtonUp(0)) // 鼠标左键按下
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 检查击中的物体是否是本对象
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
                    OnTouchExit(); // 鼠标离开物体
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
        // 点击开始时的逻辑
        ScaleObject(originalScale * scaleMultiplier);
        OnEnter?.Invoke();
    }
   
    private void OnTouchEnd()
    {
        // 点击结束时的逻辑
        Debug.Log(gameObject.name + "选择");
        ScaleObject(originalScale);
        //OnSelected?.Invoke();

    }
   
    private void OnTouchExit()
    {
        // 在这里可以选择直接还原缩放。
        // 启动协程逐渐还原
        ScaleObject(originalScale);
    }

    private void ScaleObject(Vector3 toScale)
    {
        transform.DOScale( toScale, scaleSpeed);
    }
}
