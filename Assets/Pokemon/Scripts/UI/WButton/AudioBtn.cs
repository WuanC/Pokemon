using Pokemon.Scripts.AudioManager;
using UnityEngine;

namespace Pokemon.Scripts.UI.WButton
{
    public class AudioBtn : WToggleButton
    {
        public enum BtnAudioType
        {
            Music,
            SFX,
        }
        [SerializeField] private BtnAudioType audioType;
        protected override void Start()
        {
            base.Start();
            if (audioType == BtnAudioType.Music)
            {
                btnImage.sprite = AudioService.Instance.GetMusicVolume() == 1 ? onSprite : offSprite;
            }
            else
            {
                btnImage.sprite = AudioService.Instance.GetSFXVolume() == 1 ? onSprite : offSprite;
            }

        }
        protected override void OnClick()
        {
            base.OnClick();

            if (audioType == BtnAudioType.Music)
                AudioService.Instance.ToggleMusic();
            else
                AudioService.Instance.ToggleSFX();
        }
    }
}