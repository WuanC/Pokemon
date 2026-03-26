using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class EnterHubScreen : ScreenBase
    {
        [SerializeField] private Button goBtn;
        public void Initialize(Action onGoBtnClick)
        {
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