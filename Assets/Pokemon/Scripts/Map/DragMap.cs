using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon.Scripts.Map
{
    public class DragMap : MonoBehaviour
    {
        [SerializeField] private int backgroundCount;
        [SerializeField] private float backgroundWidth;
        [SerializeField] private float pixelPerUnit;
        [SerializeField] private float cameraSpeed = 1f;

        [SerializeField] private float dragThreshold = 10f; // pixel
        private float maxCameraX;
        private float minCameraX = 0;

        private Camera mainCamera;

        private Vector3 lastMousePosition;
        private Vector3 mouseDownPosition;

        private bool isDragging = false;
        private bool isMouseDown = false;
        public Action<Vector3> OnClick;
        void Start()
        {

            mainCamera = Camera.main;
            SetCameraBounds();
        }
        public void HandleInput()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            // Mouse Down
            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
                mouseDownPosition = Input.mousePosition;
                lastMousePosition = Input.mousePosition;
                isDragging = false;
            }

            // Mouse Hold
            if (isMouseDown && Input.GetMouseButton(0))
            {

                Vector3 currentMousePos = Input.mousePosition;

                // Kiểm tra có bắt đầu drag chưa
                if (!isDragging)
                {
                    float distance = Vector3.Distance(currentMousePos, mouseDownPosition);

                    if (distance > dragThreshold)
                    {
                        isDragging = true;
                    }
                }

                // Nếu đang drag thì di chuyển camera
                if (isDragging)
                {

                    float delta = (currentMousePos.x - lastMousePosition.x) * cameraSpeed * Time.deltaTime;

                    Vector3 pos = mainCamera.transform.position;
                    pos.x = Mathf.Clamp(pos.x - delta, minCameraX, maxCameraX);
                    mainCamera.transform.position = pos;
                }

                lastMousePosition = currentMousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!isDragging)
                {
                    OnClick?.Invoke(Input.mousePosition);
                }

                isMouseDown = false;
                isDragging = false;
            }
        }



        public void SetCameraBounds()
        {
            float backgroundUnits = (backgroundWidth - 100) * backgroundCount / pixelPerUnit;
            maxCameraX = backgroundUnits - (2 * mainCamera.orthographicSize * mainCamera.aspect);
        }
    }
}