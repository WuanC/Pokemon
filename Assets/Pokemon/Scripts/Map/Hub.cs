using Pokemon.Scripts.UI.Screens;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Hub : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private float pressedScaleMultiplier = 0.95f;
        [SerializeField] private float pressedDarkenMultiplier = 0.75f;

        private Vector3 originalScale;
        private SpriteRenderer spriteRenderer;
        private Color originalColor = Color.white;
        private bool hasColorTarget;

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
            SetPressedVisual(true);
        }

        private void OnMouseUp()
        {
            SetPressedVisual(false);
        }

        private void OnMouseExit()
        {
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
            GameController.Instance.EnterHubClick(SpawnMap);
        }
        public void SpawnMap()
        {
            Map map = Instantiate(this.map, Vector2.zero, Quaternion.identity);
        }
    }
}