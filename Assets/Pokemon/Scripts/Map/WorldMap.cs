using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class WorldMap : MonoBehaviour
    {
        private DragMap dragMap;

        void Awake()
        {
            dragMap = GetComponent<DragMap>();
        }
        void Start()
        {
            dragMap.OnClick += OnClick;
        }
        void OnDestroy()
        {
            dragMap.OnClick -= OnClick;
        }
        public void OnClick(Vector3 mousePos)
        {

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("abc");
                if (hit.collider.TryGetComponent<Hub>(out Hub hub))
                {
                    hub.SpawnMap();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}