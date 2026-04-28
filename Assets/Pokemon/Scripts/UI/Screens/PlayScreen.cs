using DG.Tweening;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PlayScreen : MonoBehaviour
    {
        [SerializeField] private Button worldBtn;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI dustText;
        [SerializeField] Inventory.Inventory inventory;
        [SerializeField] private int coinToSpawnCount;
        [SerializeField] private GameObject coinGO;
        [SerializeField] private GameObject dustGO;
        public Transform coinTransform;
        void Start()
        {
            Observer.Instance.Register(EventId.OnUpdateItem, PlayScreen_OnUpdateItem);
        }
        void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnUpdateItem, PlayScreen_OnUpdateItem);
        }
        void PlayScreen_OnUpdateItem(object itemObj)
        {

            if (itemObj is Item item)
            {
                if (item.ItemBase.itemName == "Coins" || item.ItemBase.itemName == "Dusts")
                {
                    coinText.text = inventory.GetCoins()?.Quantity.ToString() ?? "0";
                    dustText.text = inventory.GetDusts()?.Quantity.ToString() ?? "0";
                }
            }

        }

        public void Initialize()
        {

            var coins = inventory.GetCoins();
            var dusts = inventory.GetDusts();
            coinText.text = coins != null ? coins.Quantity.ToString() : "0";
            dustText.text = dusts != null ? dusts.Quantity.ToString() : "0";
            worldBtn.gameObject.SetActive(false);
            worldBtn.onClick.AddListener(() =>
            {
                ScreenManager.Instance.ActiveSplashScreen(() =>
                {
                    GameController.Instance.BackToWorldMap();
                    worldBtn.gameObject.SetActive(false);
                });
            });
        }
        public void EnterDetailMap()
        {
            worldBtn.gameObject.SetActive(true);
        }
        public void AddCoinAnim(Vector3 startPos, int coinsAmount)
        {
            inventory.AddItem(Inventory.Inventory.InitCoins(coinsAmount));
            for (int i = 0; i < coinToSpawnCount; i++)
            {
                GameObject coin = MyPoolManager.Instance.GetFromPool(coinGO);
                coin.transform.position = startPos;
                int coinsToAdd = 0;
                if (i != coinToSpawnCount - 1)
                {
                    coinsToAdd = coinsAmount / coinToSpawnCount;
                    coinsAmount -= coinsToAdd;

                }
                else
                {
                    coinsToAdd = coinsAmount;
                }
                coin.transform.DOMove(coinTransform.position, 0.5f).SetEase(Ease.InBack)
                .SetLink(coin)
                .SetDelay(i * 0.1f)
                .OnComplete(() =>
                {
                    coinText.text = inventory.GetCoins()?.Quantity.ToString() ?? "0";
                    coin.gameObject.SetActive(false);
                });
            }
        }
        public bool CanPayCoins(int coinsAmount)
        {
            var coins = inventory.GetCoins();
            return coins != null && coins.Quantity >= coinsAmount;
        }
        public void PayCoins(int coinsAmount)
        {
            if (!CanPayCoins(coinsAmount)) return;
            var coins = inventory.GetCoins();
            coins.Quantity -= coinsAmount;
            coinText.text = coins.Quantity.ToString();
        }
        public bool CanPayDust(int dustAmount)
        {
            var dusts = inventory.GetDusts();
            return dusts != null && dusts.Quantity >= dustAmount;
        }
        public void PayDust(int dustAmount)
        {
            if (!CanPayDust(dustAmount)) return;
            var dusts = inventory.GetDusts();
            dusts.Quantity -= dustAmount;
            dustText.text = dusts.Quantity.ToString();
        }
    }
}