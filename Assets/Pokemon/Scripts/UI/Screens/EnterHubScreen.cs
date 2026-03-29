using System;
using DG.Tweening;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class EnterHubScreen : ScreenBase
    {
        [SerializeField] private Button goBtn;
        [SerializeField] private Image hubImage;
        [SerializeField] private TextMeshProUGUI bossCountText;
        public void Initialize(Action onGoBtnClick, Hub hub)
        {
            hubImage.sprite = hub.hubSprite;
            bossCountText.text = $"{HubSaveLoad.LoadBossAndQuest(hub.hubName)}/{hub.BossAndQuestCount}";
            base.Active();
            goBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                GameController.Instance.ActiveSplashScreen(onGoBtnClick);
            });
        }

        void OnDisable()
        {
            goBtn.onClick.RemoveAllListeners();
        }

    }
}