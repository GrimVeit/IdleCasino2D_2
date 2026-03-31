using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TouchCameraController : MonoBehaviour
{
    [Header("Настройки камеры")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float dragSpeed = 0.1f; // Уменьшена для плавности
    [SerializeField] private float minDragDistance = 10f;

    [Header("Границы")]
    [SerializeField] private Collider2D cameraBounds;

    private Vector3 lastInputPosition;
    private bool isDragging = false;
    private CinemachineConfiner2D confiner;
    private bool isTouchDevice;
    private Transform followTarget; // Добавляем target для плавного следования

    void Start()
    {
        isTouchDevice = Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer;

        if (virtualCamera != null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
            if (confiner == null)
            {
                confiner = virtualCamera.gameObject.AddComponent<CinemachineConfiner2D>();
            }

            if (cameraBounds != null)
            {
                confiner.m_BoundingShape2D = cameraBounds;
                confiner.InvalidateCache();
            }

            // Создаем пустой GameObject для следования камеры
            if (virtualCamera.Follow == null)
            {
                GameObject followObject = new GameObject("CameraFollowTarget");
                followObject.transform.position = virtualCamera.transform.position;
                followTarget = followObject.transform;
                virtualCamera.Follow = followTarget;
            }
            else
            {
                followTarget = virtualCamera.Follow;
            }
        }
    }

    void Update()
    {
        if (virtualCamera == null || followTarget == null) return;

        if (isTouchDevice)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }

        // Применяем границы к текущей позиции камеры
        if (cameraBounds != null)
        {
            Vector3 clampedPosition = ClampCameraPosition(followTarget.position);
            followTarget.position = clampedPosition;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastInputPosition = GetWorldPosition(touch.position);
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector3 currentPosition = GetWorldPosition(touch.position);
                        // Получаем разницу в мировых координатах
                        Vector3 delta = currentPosition - lastInputPosition;

                        // Двигаем камеру в противоположном направлении от движения пальца
                        MoveCamera(-delta * dragSpeed);
                        lastInputPosition = currentPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
        else if (Input.touchCount == 0)
        {
            isDragging = false;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastInputPosition = GetWorldPosition(Input.mousePosition);
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPosition = GetWorldPosition(Input.mousePosition);
            // Получаем разницу в мировых координатах
            Vector3 delta = currentPosition - lastInputPosition;

            // Двигаем камеру в противоположном направлении от движения мыши
            MoveCamera(-delta * dragSpeed);
            lastInputPosition = currentPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        if (Camera.main == null) return Vector3.zero;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0;
        return worldPos;
    }

    private void MoveCamera(Vector3 movement)
    {
        if (followTarget != null)
        {
            Vector3 newPosition = followTarget.position + movement;
            followTarget.position = newPosition;
        }
        else if (virtualCamera != null)
        {
            // Резервный вариант если followTarget не установлен
            Vector3 newPosition = virtualCamera.transform.position + movement;
            virtualCamera.transform.position = ClampCameraPosition(newPosition);
        }
    }

    private Vector3 ClampCameraPosition(Vector3 position)
    {
        if (cameraBounds == null || Camera.main == null)
            return position;

        Bounds bounds = cameraBounds.bounds;

        // Если камера использует Cinemachine с ортографической проекцией
        if (virtualCamera != null)
        {
            float orthoSize = virtualCamera.m_Lens.OrthographicSize;
            float cameraHeight = 2f * orthoSize;
            float aspect = Camera.main.aspect;
            float cameraWidth = cameraHeight * aspect;

            float minX = bounds.min.x + cameraWidth / 2f;
            float maxX = bounds.max.x - cameraWidth / 2f;
            float minY = bounds.min.y + cameraHeight / 2f;
            float maxY = bounds.max.y - cameraHeight / 2f;

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }

        return position;
    }

    // Метод для быстрого перемещения камеры к позиции
    public void SetCameraPosition(Vector3 position)
    {
        if (followTarget != null)
        {
            followTarget.position = ClampCameraPosition(position);
        }
        else if (virtualCamera != null)
        {
            virtualCamera.transform.position = ClampCameraPosition(position);
        }
    }

    public void SetCameraBounds(Collider2D bounds)
    {
        cameraBounds = bounds;
        if (confiner != null)
        {
            confiner.m_BoundingShape2D = bounds;
            confiner.InvalidateCache();
        }
    }
}
