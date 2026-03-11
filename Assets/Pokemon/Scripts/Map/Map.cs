using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Unity.Mathematics;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    [System.Serializable]
    public class Area
    {
        public Node keyNode;
        public List<Node> nodes;
        public void InitializeArena(bool isUnlock = true)
        {
            if (keyNode != null && !isUnlock)
            {
                keyNode.OnNodeCompleted += OnKeyNodeCompleted;
            }
            for (int j = 0; j < this.nodes.Count; j++)
            {
                this.nodes[j].InitializeNode();
                this.nodes[j].OnNodeCompleted += RandomPokemonInArea;
            }
        }
        private void OnKeyNodeCompleted()
        {
            UnlockArea();
            RandomPokemonInArea();

            keyNode.OnNodeCompleted -= OnKeyNodeCompleted;
        }
        public void RandomPokemonInArea()
        {
            int random = UnityEngine.Random.Range(1, 3);
            List<Node> nodes = ListUtils.ShuffleList(this.nodes);
            for (int j = 0; j < nodes.Count; j++)
            {
                if (j < random)
                {
                    nodes[j].SetHasPokemon(true);
                }
                else
                {
                    nodes[j].SetHasPokemon(false);
                }
            }
        }
        public void UnlockArea()
        {
            for (int j = 0; j < this.nodes.Count; j++)
            {
                this.nodes[j].Unlock();
            }
        }
    }
    public class Map : MonoBehaviour
    {
        [SerializeField] private int backgroundCount;
        [SerializeField] private float backgroundWidth;
        [SerializeField] private float pixelPerUnit;
        [SerializeField] private float cameraSpeed = 5f;

        [SerializeField] private float dragThreshold = 10f; // pixel
        private float maxCameraX;
        private float minCameraX = 0;

        private Camera mainCamera;

        private Vector3 lastMousePosition;
        private Vector3 mouseDownPosition;

        private bool isDragging = false;
        private bool isMouseDown = false;
        [SerializeField] private List<Area> areas;
        private Player player;
        void Awake()
        {
            player = GetComponentInChildren<Player>();
        }
        void Start()
        {
            mainCamera = Camera.main;
            SetCameraBounds();
            InitializeNode();
            player.transform.position = areas[0].nodes[0].transform.position;
            // Đặt vị trí ban đầu của player tại node đầu tiên
        }

        void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
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

            // Mouse Up
            if (Input.GetMouseButtonUp(0))
            {
                if (!isDragging)
                {
                    OnClick(Input.mousePosition);
                }

                isMouseDown = false;
                isDragging = false;
            }
        }

        private void OnClick(Vector3 mousePos)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<Node>(out Node node))
                {
                    if (node.IsLock) return;
                    player.MoveToTarget(node);
                }
            }
        }

        public void SetCameraBounds()
        {
            float backgroundUnits = backgroundWidth / pixelPerUnit;
            maxCameraX = (backgroundCount - 1) * backgroundUnits;
        }

        public void InitializeNode()
        {
            for (int i = 0; i < areas.Count; i++)
            {
                Area area = areas[i];
                if (area.keyNode == null)
                {
                    area.InitializeArena();
                    area.UnlockArea();
                    area.RandomPokemonInArea();
                }
                else
                {
                    area.InitializeArena(false);
                }

            }
        }

    }
}