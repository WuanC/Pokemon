using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.WButton
{
    public class WToggleButton : WButton
    {
        [SerializeField] protected Sprite onSprite;
        [SerializeField] protected Sprite offSprite;

        protected Image btnImage;
        protected override void Awake()
        {
            base.Awake();

            if (btnImage == null)
                btnImage = GetComponent<Image>();
        }
        protected override void OnClick()
        {
            base.OnClick();

            if (btnImage.sprite == onSprite)
                btnImage.sprite = offSprite;
            else
                btnImage.sprite = onSprite;

        }
    }
}