
using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.FReward;
using Pokemon.Scripts.MyUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private Image youWinImage;
        [SerializeField] private Image amazingImage;

        [SerializeField] private GameObject rewardContainer;
        [SerializeField] private Button nextBtn;
        Vector3 rewardContainerOriginalPos;
        [SerializeField] private RewardSlot[] rewardSlots;
        [SerializeField] private Inventory.Inventory inventory;
        void Start()
        {
            rewardContainerOriginalPos = rewardContainer.transform.position;
        }
        void OnDisable()
        {
            youWinImage.transform.DOKill();
            amazingImage.transform.DOKill();
            rewardContainer.transform.DOKill();
        }

        public void Init()
        {
            youWinImage.transform.localScale = Vector3.zero;
            amazingImage.transform.localScale = Vector3.zero;
            youWinImage.gameObject.SetActive(false);
            amazingImage.gameObject.SetActive(false);
            rewardContainer.SetActive(false);
            nextBtn.onClick.RemoveAllListeners();
        }
        public IEnumerator ShowWinScreen(Reward reward)
        {
            Init();
            gameObject.SetActive(true);
            youWinImage.gameObject.SetActive(true);
            yield return youWinImage.transform.DOScale(Vector3.one, 0.5f).WaitForCompletion();
            amazingImage.gameObject.SetActive(true);
            yield return amazingImage.transform.DOScale(Vector3.one, 0.5f).WaitForCompletion();

            yield return new WaitForSeconds(0.5f);
            if (reward != null) InitializeReward(reward);
            rewardContainer.transform.position = rewardContainerOriginalPos + Vector3.down * 10;
            rewardContainer.SetActive(true);
            yield return rewardContainer.transform.DOMove(rewardContainerOriginalPos, 0.5f).WaitForCompletion();

            nextBtn.onClick.AddListener(() =>
            {
                foreach (var item in reward.items)
                {
                    inventory.AddItem(item);
                }
                gameObject.SetActive(false);
                Observer.Instance.Broadcast(EventId.OnEndBattle, true);
            });
        }
        public void InitializeReward(Reward reward)
        {
            for (int i = 0; i < rewardSlots.Length; i++)
            {
                if (i < reward.items.Count)
                {
                    rewardSlots[i].gameObject.SetActive(true);
                    rewardSlots[i].Initialize(reward.items[i].ItemBase.icon, reward.items[i].Quantity);
                }
                else
                {
                    rewardSlots[i].gameObject.SetActive(false);
                }
            }
        }
    }
}