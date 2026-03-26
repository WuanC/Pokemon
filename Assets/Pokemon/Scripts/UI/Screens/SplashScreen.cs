using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private Image bgImage;
        [SerializeField] private float fadeDuration = 1f;

        public Action onFadeComplete;
        public void Fade()
        {
            gameObject.SetActive(true);
            bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0f);
            bgImage.DOFade(1f, fadeDuration / 2).OnComplete(() =>
            {
                onFadeComplete?.Invoke();
                bgImage.DOFade(0f, fadeDuration / 2).OnComplete(() =>
                {
                    onFadeComplete = null;
                    gameObject.SetActive(false);
                });
            });
        }
    }
}