using System;
using DG.Tweening;
using Pokemon.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class MorePanel : MonoBehaviour
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button catchBtn;
        [SerializeField] Button runBtn;
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        [SerializeField] PartyContainer partyContainer;
        [SerializeField] BattleController battleController;
        public void Start()
        {
            runBtn.onClick.AddListener(OnRun);
            catchBtn.onClick.AddListener(OnCatch);
            backBtn.onClick.AddListener(() =>
            {
                battleController.OpenMainPanel(null);
            });
        }

        void OnDestroy()
        {
            runBtn.onClick.RemoveListener(OnRun);
            catchBtn.onClick.RemoveListener(OnCatch);
            backBtn.onClick.RemoveAllListeners();
        }
        public void EnablePanel(float duration, bool forceSelect)
        {
            if (forceSelect)
            {
                catchBtn.gameObject.SetActive(false);
                backBtn.gameObject.SetActive(false);
            }
            else
            {
                catchBtn.gameObject.SetActive(true);
                backBtn.gameObject.SetActive(true);
            }
            gameObject.SetActive(true);
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainer.DOAnchorPosY(btnStartPos.y, duration);
            partyContainer.OpenParty(duration);
        }
        public void DisablePanel(float duration, Action onComplete)
        {
            partyContainer.CloseParty(duration);
            btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
        public void OnRun()
        {
            battleController.RunBattle();
        }
        public void OnCatch()
        {
            battleController.OpenMainPanel(() =>
            {
                battleController.CatchPokemon();
            });

        }

    }
}