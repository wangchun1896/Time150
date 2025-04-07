using UnityEngine;

public class RotateAndDrag : MonoBehaviour
{
    private bool isDragging = false; // �Ƿ������϶�
    private Vector3 lastMousePosition; // ��һ֡�����λ��
    private float rotationSpeed = 20f; // Ĭ����ת�ٶ�
    private Vector3 lastDragDirection;

    private void OnEnable()
    {
        lastDragDirection = Vector3.down+Vector3.one*(-0.2f); // ��ʼ������϶�����
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // �༭���Ͷ���ƽ̨ʹ���������
        if (IsMouseOverObject())
        {
            if (Input.GetMouseButton(0))
            {
                DragMouse();
            }
            else if (isDragging)
            {
                isDragging = false; // ֹͣ�϶�
            }
        }
        else if (isDragging)
        {
            isDragging = false; // ֹͣ�϶�
        }
#else
        // �ƶ�ƽ̨ʹ�ô�������
        if (IsTouchOverObject())
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                
                DragTouch();
            }
            else if (isDragging && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false; // ֹͣ�϶�
            }
        }
        else if (isDragging)
        {
            isDragging = false; // ֹͣ�϶�
        }
#endif
        // ������ת��ֻ�е�û���϶�ʱ��
        if (!isDragging)
            Rotate();
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void DragMouse()
    {
        // ��ȡ��ǰ�����λ��
        Vector3 currentMousePosition = Input.mousePosition;

        if (!isDragging)
        {
            lastMousePosition = currentMousePosition;
            isDragging = true;
            return;
        }

        Vector3 offset = currentMousePosition - lastMousePosition;
        float mouseDragRotationSpeed = 0.1f; // �����϶���ת�ٶ�
        float rotationX = offset.y * mouseDragRotationSpeed; // ������� Y �ƶ�˳ʱ����ת
        float rotationY = -offset.x * mouseDragRotationSpeed; // ������� X �ƶ���ʱ����ת

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
        // ��ȡ��ǰ�Ĵ���λ��
        Vector3 currentTouchPosition = Input.GetTouch(0).position;

        if (!isDragging)
        {
            lastMousePosition = currentTouchPosition;
            isDragging = true;
            return;
        }

        Vector3 offset = currentTouchPosition - lastMousePosition;
        float touchDragRotationSpeed = 0.1f; // �����϶���ת�ٶ�
        float rotationX = offset.y * touchDragRotationSpeed; // ���ڴ��� Y �ƶ�˳ʱ����ת
        float rotationY = -offset.x * touchDragRotationSpeed; // ���ڴ��� X �ƶ���ʱ����ת

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
            return false; // ������λ������Ļ���������
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform; // ֻ�е��������е�ǰ����ʱ���� true
        }

        return false; // û�����е�ǰ����
    }

    bool IsTouchOverObject()
    {
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform; // ֻ�е��������е�ǰ����ʱ���� true
            }
        }

        return false; // û�����е�ǰ�����û�д���
    }
}
