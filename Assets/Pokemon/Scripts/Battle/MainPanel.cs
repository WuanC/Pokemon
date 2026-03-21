using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Pokemon.Scripts.Battle
{
    public class MainPanel : MonoBehaviour
    {
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        public void EnablePanel(float duration)
        {
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainer.DOAnchorPosY(btnStartPos.y, duration);
        }
        public void DisablePanel(float duration, Action onComplete)
        {
            btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}