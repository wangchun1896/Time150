using UnityEngine;

public class RotateAndDrag : MonoBehaviour
{
    private bool isDragging = false; // 是否正在拖动
    private Vector3 lastMousePosition; // 上一帧的鼠标位置
    private float rotationSpeed = 20f; // 默认旋转速度
    private Vector3 lastDragDirection;

    private void OnEnable()
    {
        lastDragDirection = Vector3.down+Vector3.one*(-0.2f); // 初始化最后拖动方向
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 编辑器和独立平台使用鼠标输入
        if (IsMouseOverObject())
        {
            if (Input.GetMouseButton(0))
            {
                DragMouse();
            }
            else if (isDragging)
            {
                isDragging = false; // 停止拖动
            }
        }
        else if (isDragging)
        {
            isDragging = false; // 停止拖动
        }
#else
        // 移动平台使用触摸输入
        if (IsTouchOverObject())
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                
                DragTouch();
            }
            else if (isDragging && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false; // 停止拖动
            }
        }
        else if (isDragging)
        {
            isDragging = false; // 停止拖动
        }
#endif
        // 持续旋转（只有当没有拖动时）
        if (!isDragging)
            Rotate();
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void DragMouse()
    {
        // 获取当前的鼠标位置
        Vector3 currentMousePosition = Input.mousePosition;

        if (!isDragging)
        {
            lastMousePosition = currentMousePosition;
            isDragging = true;
            return;
        }

        Vector3 offset = currentMousePosition - lastMousePosition;
        float mouseDragRotationSpeed = 0.1f; // 调整拖动旋转速度
        float rotationX = offset.y * mouseDragRotationSpeed; // 基于鼠标 Y 移动顺时针旋转
        float rotationY = -offset.x * mouseDragRotationSpeed; // 基于鼠标 X 移动逆时针旋转

        transform.Rotate(Vector3.right, rotationX, Space.World);
        transform.Rotate(Vector3.up, rotationY, Space.World);

        if (new Vector3(rotationX, rotationY, 0).normalized != Vector3.zero)
        {
            lastDragDirection = new Vector3(rotationX, rotationY, 0).normalized;
        }

        lastMousePosition = currentMousePosition;
    }
#else
    void DragTouch()
    {
        // 获取当前的触摸位置
        Vector3 currentTouchPosition = Input.GetTouch(0).position;

        if (!isDragging)
        {
            lastMousePosition = currentTouchPosition;
            isDragging = true;
            return;
        }

        Vector3 offset = currentTouchPosition - lastMousePosition;
        float touchDragRotationSpeed = 0.1f; // 调整拖动旋转速度
        float rotationX = offset.y * touchDragRotationSpeed; // 基于触摸 Y 移动顺时针旋转
        float rotationY = -offset.x * touchDragRotationSpeed; // 基于触摸 X 移动逆时针旋转

        transform.Rotate(Vector3.right, rotationX, Space.World);
        transform.Rotate(Vector3.up, rotationY, Space.World);

        if (new Vector3(rotationX, rotationY, 0).normalized != Vector3.zero)
        {
            lastDragDirection = new Vector3(rotationX, rotationY, 0).normalized;
        }

        lastMousePosition = currentTouchPosition;
    }
#endif

    void Rotate()
    {
        transform.Rotate(lastDragDirection, rotationSpeed * Time.deltaTime, Space.World);
    }

    bool IsMouseOverObject()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return false; // 如果鼠标位置在屏幕外则不做检测
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform; // 只有当射线命中当前对象时返回 true
        }

        return false; // 没有命中当前对象
    }

    bool IsTouchOverObject()
    {
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform; // 只有当射线命中当前对象时返回 true
            }
        }

        return false; // 没有命中当前对象或没有触摸
    }
}
