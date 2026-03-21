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
        public void EnablePanel()
        {
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainer.DOAnchorPosY(btnStartPos.y, 0.25f);
        }
        public void DisablePanel()
        {
            btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, 0.25f).OnComplete(() =>
            {
                onDisableCompleted?.Invoke();
            });
        }
    }
}