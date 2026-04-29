using System;
using DG.Tweening;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.UI;
using Pokemon.Scripts.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class MorePanel : MonoBehaviour
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button catchBtn;
        [SerializeField] TextMeshProUGUI discatchCount;
        [SerializeField] Button itemBtn;
        [SerializeField] Button runBtn;
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        [SerializeField] PartyContainer partyContainer;
        [SerializeField] BattleController battleController;
        [SerializeField] InventoryScreen inventoryScreen;
        private Item discatchItem;
        public void Start()
        {
            runBtn.onClick.AddListener(OnRun);
            catchBtn.onClick.AddListener(OnCatch);
            backBtn.onClick.AddListener(() =>
            {
                battleController.OpenMainPanel(null);
            });
            itemBtn.onClick.AddListener(() =>
            {
                battleController.OpenMainPanel(() =>
                {
                    inventoryScreen.Initialize();
                }, 0);

            });
        }

        void OnDestroy()
        {
            runBtn.onClick.RemoveListener(OnRun);
            catchBtn.onClick.RemoveListener(OnCatch);
            backBtn.onClick.RemoveAllListeners();
            itemBtn.onClick.RemoveAllListeners();
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
            SetupDiscatch();
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
        public void SetupDiscatch()
        {
            Item discatch = Inventory.Inventory.Instance.GetDiscatch();
            if (discatch != null)
            {
                catchBtn.gameObject.SetActive(true);
                discatchCount.text = discatch.Quantity.ToString();
                discatchItem = discatch;

            }
            else
            {
                catchBtn.gameObject.SetActive(false);
                discatchItem = null;
            }
        }
        public void OnRun()
        {
            battleController.RunBattle();
        }
        public void OnCatch()
        {
            battleController.OpenMainPanel(() =>
            {
                if (discatchItem != null && discatchItem.ItemBase is CatchItem)
                {
                    Observer.Instance.Broadcast(EventId.OnItemDiscatchUsed, discatchItem);
                }
            });

        }

    }
}