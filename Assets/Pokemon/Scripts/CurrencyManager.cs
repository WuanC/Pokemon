using System;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;

namespace Pokemon.Scripts
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        private int coins;
        [SerializeField] private int coinToSpawnCount;
        [SerializeField] private PlayScreen playScreen;
        [SerializeField] private GameObject coinGO;
        public void AddCoinAnim(Vector3 startPos, int coinsAmount)
        {
            coins += coinsAmount;
            int prevCoinsAdd = 0;
            for (int i = 0; i < coinToSpawnCount; i++)
            {
                GameObject coin = MyPoolManager.Instance.GetFromPool(coinGO, transform);
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
                prevCoinsAdd += coinsToAdd;
                coin.transform.DOMove(playScreen.coinTransform.position, 0.5f).SetEase(Ease.InBack)
                .SetLink(coin)
                .SetDelay(i * 0.1f)
                .OnComplete(() =>
                {
                    playScreen.UpdateCoinText(prevCoinsAdd);
                    coin.gameObject.SetActive(false);
                });
            }
        }
    }
}