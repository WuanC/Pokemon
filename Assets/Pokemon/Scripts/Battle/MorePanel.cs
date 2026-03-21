using DG.Tweening;
using Pokemon.Scripts.Party;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class MorePanel : MonoBehaviour
    {
        [SerializeField] Button catchBtn;
        [SerializeField] Button runBtn;
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        [SerializeField] PartyContainer partyContainer;
        public UnityEvent onDisableCompleted;
        public void EnablePanel()
        {
            gameObject.SetActive(true);
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainer.DOAnchorPosY(btnStartPos.y, 0.25f);
            partyContainer.OpenParty(0.25f);
        }
        public void DisablePanel()
        {
            partyContainer.CloseParty(0.25f);
            btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, 0.25f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                onDisableCompleted?.Invoke();
            });
        }

    }
}