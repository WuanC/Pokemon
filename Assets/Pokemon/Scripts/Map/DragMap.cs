using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon.Scripts.Map
{
    public class DragMap : MonoBehaviour
    {
        [SerializeField] private Vector2 backgroundPositionX;
        [SerializeField] private float cameraSpeed = 1f;

        [SerializeField] private float dragThreshold = 10f; // pixel
        private float maxCameraX;
        private float minCameraX = 0;

        private Camera mainCamera;

        private Vector3 lastMousePosition;
        private Vector3 mouseDownPosition;

        private bool isDragging = false;
        private bool isMouseDown = false;
        public Action<Vector3, int> OnClick;
        private bool startedOverUI;
        void Start()
        {

            mainCamera = Camera.main;
            SetCameraBounds();
            Camera.main.transform.position = new Vector3(minCameraX, 0f, Camera.main.transform.position.z);
        }
        void OnEnable()
        {
            Camera.main.transform.position = new Vector3(minCameraX, 0f, Camera.main.transform.position.z);
        }
        public void HandleInput()
        {
            if (Input.touchCount <= 0)
                return;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startedOverUI = EventSystem.current != null &&
                                    EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                    if (startedOverUI)
                        return;
                    isMouseDown = true;
                    mouseDownPosition = touch.position;
                    lastMousePosition = touch.position;
                    isDragging = false;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (startedOverUI)
                        return;
                    if (!isMouseDown)
                        return;

                    Vector3 currentPos = touch.position;

                    if (!isDragging)
                    {
                        float distance = Vector3.Distance(currentPos, mouseDownPosition);

                        if (distance > dragThreshold)
                            isDragging = true;
                    }

                    if (isDragging)
                    {
                        float delta = (currentPos.x - lastMousePosition.x) * cameraSpeed * Time.deltaTime;

                        Vector3 pos = mainCamera.transform.position;
                        pos.x = Mathf.Clamp(pos.x - delta, minCameraX, maxCameraX);

                        mainCamera.transform.position = pos;
                    }

                    lastMousePosition = currentPos;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (!startedOverUI && !isDragging)
                    {
                        OnClick?.Invoke(touch.position, touch.fingerId);
                    }

                    isMouseDown = false;
                    isDragging = false;
                    startedOverUI = false;
                    break;
            }
        }

        public void SetCameraBounds()
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

            minCameraX =
                backgroundPositionX.x + cameraHalfWidth;

            maxCameraX =
                backgroundPositionX.y - cameraHalfWidth;
        }
    }
}