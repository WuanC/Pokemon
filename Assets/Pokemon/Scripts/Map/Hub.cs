using Pokemon.Scripts.AudioManager;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Saving;
using Pokemon.Scripts.Tutorial;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon.Scripts.Map
{
    public class Hub : MonoBehaviour
    {
        [SerializeField] private MapData mapData;
        [SerializeField] private float pressedScaleMultiplier = 0.95f;
        [SerializeField] private float pressedDarkenMultiplier = 0.75f;

        private Vector3 originalScale;
        private SpriteRenderer spriteRenderer;
        private Color originalColor = Color.white;
        private bool hasColorTarget;

        public MapData MapData => mapData;

        private void Awake()
        {
            originalScale = transform.localScale;

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
                hasColorTarget = true;
                return;
            }
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            SetPressedVisual(true);
        }

        private void OnMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            SetPressedVisual(false);
        }

        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            SetPressedVisual(false);
        }

        private void SetPressedVisual(bool isPressed)
        {
            transform.localScale = isPressed
                ? originalScale * pressedScaleMultiplier
                : originalScale;

            if (!hasColorTarget)
            {
                return;
            }

            Color targetColor = isPressed
                ? originalColor * pressedDarkenMultiplier
                : originalColor;
            targetColor.a = originalColor.a;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = targetColor;
                return;
            }

        }


        public void HubClick()
        {

            if (!TutorialManager.IsTutorialCompleted() && GameController.Instance.enableHub != this)
            {
                Debug.Log("Wrong hub");
                Observer.Instance.Broadcast(EventId.OnShowMessage, "You must complete the tutorial first!");
                return;
            }
            if (mapData.mapCondition != null)
            {
                if (HubController.Instance.ContainsHub(mapData.hubName))
                {
                    ScreenManager.Instance.EnterHubClick(SpawnMap, mapData);
                    return;
                }
                else if (mapData.mapCondition.conditionType == MapConditionType.UnlockMap)
                {
                    int bossInConditionMap = HubSaveLoad.LoadBoss(mapData.mapCondition.requiredMap.hubName);
                    int targetBossInConditionMap = mapData.mapCondition.requiredMap.mapPrefab.npcBattle.Length;
                    if (bossInConditionMap == targetBossInConditionMap)
                    {
                        HubController.Instance.UnlockHub(mapData.hubName);
                        ScreenManager.Instance.EnterHubClick(SpawnMap, mapData);
                        return;
                    }

                }

                ScreenManager.Instance.EnterHubClick(SpawnMap, mapData, mapData.mapCondition);


            }
            else
            {

                ScreenManager.Instance.EnterHubClick(SpawnMap, mapData);
            }
        }
        public void SpawnMap()
        {
            AudioService.Instance.PlayMusic(AudioId.BGM_Hub);
            Map map = Instantiate(mapData.mapPrefab, Vector2.zero, Quaternion.identity);
            map.InitializeMap(mapData);
        }
    }
}