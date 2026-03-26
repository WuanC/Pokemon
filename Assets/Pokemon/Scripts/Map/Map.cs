using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Map
{

    public class Map : MonoBehaviour
    {

        [SerializeField] private List<Area> areas;
        private DragMap dragMap;
        private Player player;
        private Camera mainCamera;
        void Awake()
        {
            player = GetComponentInChildren<Player>();
            dragMap = GetComponent<DragMap>();
        }
        void Start()
        {
            mainCamera = Camera.main;
            GameController.Instance.MapRegister(dragMap);
            dragMap.OnClick += OnClick;
            InitializeNode();
            player.transform.position = areas[0].nodes[0].transform.position;
        }
        void OnDestroy()
        {
            dragMap.OnClick -= OnClick;
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