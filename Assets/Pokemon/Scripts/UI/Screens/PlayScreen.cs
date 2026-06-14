using DG.Tweening;
using Game;
using Pokemon.Scripts.AudioManager;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Pokemon.Scripts.Pokemon;
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
        [SerializeField] private int coinToSpawnCount;
        [SerializeField] private GameObject coinGO;
        [SerializeField] private GameObject dustGO;
        [SerializeField] private Button pkmDexBtn;
        [SerializeField] private TextMeshProUGUI pkmDexCountText;
        public Transform coinTransform;
        void Start()
        {
            Observer.Instance.Register(EventId.OnUpdateItem, PlayScreen_OnUpdateItem);
            pkmDexBtn.onClick.AddListener(() =>
            {
                ScreenManager.Instance.ActivePokemonDexScreen();
            });
            Observer.Instance.Register(EventId.OnAddPokemon, PlayScreen_OnAddPokemon);
        }
        void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnUpdateItem, PlayScreen_OnUpdateItem);
            pkmDexBtn.onClick.RemoveAllListeners();
            Observer.Instance.Unregister(EventId.OnAddPokemon, PlayScreen_OnAddPokemon);
        }
        void PlayScreen_OnUpdateItem(object itemObj)
        {

            if (itemObj is Item item)
            {
                if (item.ItemBase.itemName == "Coins" || item.ItemBase.itemName == "Dusts")
                {
                    coinText.text = NumberFormatter.Format(Inventory.Inventory.Instance.GetCoins()?.Quantity ?? 0);
                    dustText.text = NumberFormatter.Format(Inventory.Inventory.Instance.GetDusts()?.Quantity ?? 0);
                }
            }

        }

        public void Initialize()
        {
            var coins = Inventory.Inventory.Instance.GetCoins();
            var dusts = Inventory.Inventory.Instance.GetDusts();
            coinText.text = NumberFormatter.Format(coins?.Quantity ?? 0);
            dustText.text = dusts != null ? dusts.Quantity.ToString() : "0";
            worldBtn.gameObject.SetActive(false);
            worldBtn.onClick.AddListener(() =>
            {
                ScreenManager.Instance.ActiveSplashScreen(() =>
                {
                    GameController.Instance.BackToWorldMap();
                    worldBtn.gameObject.SetActive(false);
                    AudioService.Instance.PlayMusic(AudioId.BGM_Game);
                });
            });
            int caughtCount = PlayerParty.Instance.pokedex.Count;
            int totalCount = PokemonDB.GetAllPokemon().Count;
            pkmDexCountText.text = $"{caughtCount}/{totalCount}";
        }
        private void PlayScreen_OnAddPokemon(object data)
        {
            if (data is string pokemonName)
            {
                int caughtCount = PlayerParty.Instance.pokedex.Count;
                int totalCount = PokemonDB.GetAllPokemon().Count;
                pkmDexCountText.text = $"{caughtCount}/{totalCount}";
            }
        }
        public void EnterDetailMap()
        {
            worldBtn.gameObject.SetActive(true);
        }
        public void AddCoinAnim(Vector3 startPos, int coinsAmount)
        {
            Inventory.Inventory.Instance.AddItem(Inventory.Inventory.InitCoins(coinsAmount));
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
                    coinText.text = NumberFormatter.Format(Inventory.Inventory.Instance.GetCoins()?.Quantity ?? 0);
                    coin.gameObject.SetActive(false);
                });
            }
        }

    }
}