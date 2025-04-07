using UnityEngine;

public class Drag3DObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;

    void Start()
    {
        mainCamera = GameObject.Find("ModelCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // 处理鼠标事件
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown();
        }
        if (Input.GetMouseButton(0))
        {
            OnMouseButtonDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }

        // 处理触摸事件
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(touch.position);
                    break;
                case TouchPhase.Moved:
                    OnTouchMoved(touch.position);
                    break;
                case TouchPhase.Ended:
                    OnTouchEnded();
                    break;
            }
        }
    }

    private void OnMouseButtonDown()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            }
        }
    }

    private void OnMouseButtonDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            transform.position = newPosition + offset;
        }
    }

    private void OnMouseButtonUp()
    {
        isDragging = false;
    }

    private void OnTouchBegan(Vector2 touchPosition)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            }
        }
    }

    private void OnTouchMoved(Vector2 touchPosition)
    {
        if (isDragging)
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.WorldToScreenPoint(transform.position).z));
            transform.position = newPosition + offset;
        }
    }

    private void OnTouchEnded()
    {
        isDragging = false;
    }
}