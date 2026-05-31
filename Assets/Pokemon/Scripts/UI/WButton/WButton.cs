using Pokemon.Scripts.AudioManager;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.WButton
{
    public class WButton : MonoBehaviour
    {
        protected Button button;

        protected virtual void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();
        }
        protected virtual void Start()
        {
            button.onClick.AddListener(OnClick);
        }
        protected void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }
        protected virtual void OnClick()
        {
            AudioService.Instance.PlaySFX(AudioId.BtnClick);
        }
    }
}