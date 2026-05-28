using DG.Tweening;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public class ScaleLoop : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float maxScale = 1f;
        [SerializeField] private float duration = 0.5f;

        private Tween scaleTween;

        private void OnEnable()
        {
            transform.localScale = Vector3.one * minScale;

            scaleTween = transform
                .DOScale(maxScale, duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            scaleTween?.Kill();
        }
    }
}