using System.Collections.Generic;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Map
{

    public class Map : MonoBehaviour
    {
        [SerializeField] private List<Area> areas;
        public string HubName { get; private set; }
        private DragMap dragMap;
        private Player player;
        private Camera mainCamera;
        public List<PokemonData> pokemonInMaps { get; private set; }
        void Awake()
        {
            player = GetComponentInChildren<Player>();
            dragMap = GetComponent<DragMap>();
        }
        public void InitializeMap(MapData mapData)
        {
            HubName = mapData.hubName;
            pokemonInMaps = mapData.pokemonInMaps;
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
                    area.InitializeArena(this, i);
                    area.UnlockArea();
                    area.RandomPokemonInArea();
                }
                else
                {

                    if (TrainerSaveLoad.LoadTrainerData(area.keyNode.NodeName) != 0)
                    {
                        area.InitializeArena(this, i);
                        area.UnlockArea();
                        area.RandomPokemonInArea();
                    }
                    else area.InitializeArena(this, i, false);
                }
            }
        }
    }

}
