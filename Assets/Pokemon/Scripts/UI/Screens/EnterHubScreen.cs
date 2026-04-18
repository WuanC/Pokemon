using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class EnterHubScreen : ScreenBase
    {
        private MapData mapData;
        [Header("Unlock pannel")]
        [SerializeField] private Button goBtn;
        [SerializeField] private Image hubImage;
        [SerializeField] private TextMeshProUGUI bossCountText;
        [SerializeField] private TextMeshProUGUI pokemonCountText;
        [SerializeField] private PokemonLocker pokemonLockerPrefab;
        [SerializeField] private Transform pokemonLockerContainer;

        [SerializeField] private TextMeshProUGUI goldRewardText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressFillImage;
        List<PokemonLocker> pokemonLockers = new();
        [Header("Lock pannel")]
        [SerializeField] private GameObject lockPannel;
        [SerializeField] private Button exitLockPannelBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private TextMeshProUGUI goldRequiredText;
        [SerializeField] private TextMeshProUGUI description;
        protected override void Start()
        {
            base.Start();
            exitLockPannelBtn.onClick.AddListener(LockPanelDeactive);
        }
        public void Initialize(Action onGoBtnClick, MapData mapData, MapConditionData mapCondition = null)
        {
            this.mapData = mapData;
            if (mapCondition == null)
            {
                ActivePannelUnlock(onGoBtnClick);
            }
            else
            {
                gameObject.SetActive(true);
                lockPannel.SetActive(true);
                description.text = mapCondition.description;
                if (mapCondition.conditionType == MapConditionType.Pay)
                {
                    buyBtn.gameObject.SetActive(true);
                    goldRequiredText.text = mapCondition.goldRequired.ToString();
                    buyBtn.onClick.AddListener(() =>
                    {
                        if (CurrencyManager.Instance.CanPay(mapCondition.goldRequired))
                        {
                            CurrencyManager.Instance.Pay(mapCondition.goldRequired);
                            LockPanelDeactive();
                            HubController.Instance.UnlockHub(mapData.hubName);
                            ActivePannelUnlock(onGoBtnClick);
                        }
                    });
                }
                else
                {
                    buyBtn.gameObject.SetActive(false);
                }

            }

        }
        public void ActivePannelUnlock(Action onGoBtnClick)
        {
            hubImage.sprite = mapData.headerMap;
            UpdateProgress(0, 0);
            goldRewardText.text = $"{mapData.goldReward}";
            InitPokemonLocker(mapData.pokemonInMaps);
            base.Active();
            goBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                ScreenManager.Instance.ActiveSplashScreen(onGoBtnClick);
            });
        }
        public void InitPokemonLocker(List<PokemonData> pokemonDatas)
        {
            for (int i = 0; i < pokemonDatas.Count; i++)
            {
                PokemonData pkmData = pokemonDatas[i];
                GameObject locker = MyPoolManager.Instance.GetFromPool(pokemonLockerPrefab.gameObject, pokemonLockerContainer);
                locker.transform.SetSiblingIndex(i);
                PokemonLocker pokemonLocker = locker.GetComponent<PokemonLocker>();
                pokemonLocker.Initialize(pkmData, isLocker: true);
                pokemonLockers.Add(pokemonLocker);
            }
        }
        void OnDisable()
        {
            goBtn.onClick.RemoveAllListeners();
            foreach (var locker in pokemonLockers)
            {
                locker.gameObject.SetActive(false);
            }
            pokemonLockers.Clear();
        }
        public void UpdateProgress(int bossCount, int pokemonCount)
        {
            bossCountText.text = $"{bossCount}/{mapData.bossAndQuestCount}";
            pokemonCountText.text = $"{pokemonCount}/{mapData.pokemonInMaps.Count}";
            progressText.text = $"{(bossCount + pokemonCount) * 100 / (mapData.bossAndQuestCount + mapData.pokemonInMaps.Count)}% COMPLETED";
            progressFillImage.fillAmount = (float)(bossCount + pokemonCount) / (mapData.bossAndQuestCount + mapData.pokemonInMaps.Count);
        }
        public void LockPanelDeactive()
        {
            lockPannel.SetActive(false);
            gameObject.SetActive(false);
            buyBtn.onClick.RemoveAllListeners();
            buyBtn.gameObject.SetActive(false);
        }

    }
}