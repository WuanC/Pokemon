using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.Map;
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
        [SerializeField] private Button goBtn;
        [SerializeField] private Image hubImage;
        [SerializeField] private TextMeshProUGUI bossCountText;
        [SerializeField] private TextMeshProUGUI pokemonCountText;
        [SerializeField] private PokemonLocker pokemonLockerPrefab;
        [SerializeField] private Transform pokemonLockerContainer;
        List<PokemonLocker> pokemonLockers = new();
        public void Initialize(Action onGoBtnClick, MapData mapData)
        {
            hubImage.sprite = mapData.headerMap;
            bossCountText.text = $"{0}/{mapData.bossAndQuestCount}";
            pokemonCountText.text = $"{0}/{mapData.pokemonInMaps.Count}";
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

    }
}