using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Pokemon.Scripts.Battle
{
    public class MainPanel : MonoBehaviour
    {
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        public UnityEvent onDisableCompleted;
        public void EnablePanel(float duration = 0.25f)
        {
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainer.DOAnchorPosY(btnStartPos.y, duration);
        }
        public void DisablePanel(float duration = 0.25f)
        {
            btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, duration).OnComplete(() =>
            {
                onDisableCompleted?.Invoke();
            });
        }
    }
}