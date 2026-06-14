
using System;
using DG.Tweening;
using Pokemon.Scripts.FReward;
using UnityEngine;
using UnityEngine.UI;


namespace Pokemon.Scripts.Tutorial
{
    public class RewardTutorialScreen : MonoBehaviour
    {
        [SerializeField] private Reward rewardConfig;
        [SerializeField] private RewardSlot[] rewardSlots;

        [SerializeField] private Button closeBtn;

        [SerializeField] private GameObject rewardVisual;
        public event Action OnClose;
        void OnEnable()
        {
            rewardVisual.transform.localScale = Vector3.zero;
            rewardVisual.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        private void Start()
        {
            for (int i = 0; i < rewardConfig.items.Count; i++)
            {
                if (i < rewardSlots.Length)
                {
                    rewardSlots[i].Initialize(rewardConfig.items[i].ItemBase.icon, rewardConfig.items[i].Quantity);
                }
            }
            closeBtn.onClick.AddListener(() =>
            {
                foreach (var item in rewardConfig.items)
                {
                    Inventory.Inventory.Instance.AddItem(item);
                }
                OnClose?.Invoke();
                gameObject.SetActive(false);

            });
        }

        private void OnDestroy()
        {
            closeBtn.onClick.RemoveAllListeners();
        }


    }
}