
using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class BattlePokemon : MonoBehaviour
    {
        [SerializeField] bool isPlayerPokemon;
        [SerializeField] Image pokemonImage;
        [SerializeField] Vector3 originalPosition;
        [SerializeField] BattlePokemonUI pokemonUI;
        [SerializeField] private RectTransform statContainer;
        [SerializeField] private Image attackProgress;
        [SerializeField] private Image defenseProgress;
        [SerializeField] private Image speedProgress;
        [SerializeField] private Image accuracyProgress;
        public PokemonUnit Pokemon { get; private set; }
        public bool IsPlayerPokemon => isPlayerPokemon;
        void Start()
        {
            originalPosition = pokemonImage.transform.localPosition;
        }
        public void SetPokemon(PokemonUnit pokemon, Action<int> onSkillSelected = null, float duration = 0.5f)
        {

            Pokemon = pokemon;
            if (isPlayerPokemon)
            {
                pokemonImage.sprite = pokemon.Data.backSprite;
            }
            else
            {
                pokemonImage.sprite = pokemon.Data.frontSprite;
            }
            pokemonUI.SetPokemon(pokemon, onSkillSelected);
            EnterAnimation(duration);
        }
        public Tween UpdateHp(float hpFraction, float duration)
        {
            return pokemonUI.UpdateHP(hpFraction, duration);
        }

        public void EnterAnimation(float duration)
        {
            if (isPlayerPokemon)
            {
                pokemonImage.transform.localPosition = new Vector3(-1000, originalPosition.y);
            }
            else
            {
                pokemonImage.transform.localPosition = new Vector3(1000, originalPosition.y);
            }
            pokemonImage.transform.DOLocalMoveX(originalPosition.x, duration);
        }
        public void ExitAnimation(Action onComplete, float duration)
        {
            Vector3 targetPos;
            if (isPlayerPokemon)
            {
                targetPos = new Vector3(-1000, originalPosition.y);
            }
            else
            {
                targetPos = new Vector3(1000, originalPosition.y);
            }
            pokemonImage.transform.DOLocalMove(targetPos, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        public void UpdateStatUI(StatBoost statBoosts, float previousValue, float currentValue, float duration)
        {
            float maxValue = Pokemon.GetStat(statBoosts.stat) * 4f;
            if (statBoosts.stat == Stat.Attack)
            {
                Transform parent = attackProgress.transform.parent.parent;

                attackProgress.fillAmount = previousValue / maxValue;
                parent.gameObject.SetActive(true);
                attackProgress.DOFillAmount(currentValue / maxValue, duration).OnComplete(() =>
                {
                    parent.gameObject.SetActive(false);
                });
            }
            else if (statBoosts.stat == Stat.Defense)
            {
                Transform parent = defenseProgress.transform.parent.parent;

                defenseProgress.fillAmount = previousValue / maxValue;
                parent.gameObject.SetActive(true);
                defenseProgress.DOFillAmount(currentValue / maxValue, duration).OnComplete(() =>
                {
                    parent.gameObject.SetActive(false);
                });
            }
            else if (statBoosts.stat == Stat.Speed)
            {
                Transform parent = speedProgress.transform.parent.parent;

                speedProgress.fillAmount = previousValue / maxValue;
                parent.gameObject.SetActive(true);
                speedProgress.DOFillAmount(currentValue / maxValue, duration).OnComplete(() =>
                {
                    parent.gameObject.SetActive(false);
                });
            }
            else if (statBoosts.stat == Stat.Accuracy)
            {
                Transform parent = accuracyProgress.transform.parent.parent;
                accuracyProgress.fillAmount = previousValue / maxValue;
                parent.gameObject.SetActive(true);
                accuracyProgress.DOFillAmount(currentValue / maxValue, duration).OnComplete(() =>
                {
                    parent.gameObject.SetActive(false);
                });
            }
        }




        public Sequence AttackAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            if (isPlayerPokemon)
            {
                sequence.Append(pokemonImage.transform.DOLocalMoveX(originalPosition.x + 50, 0.25f));
            }
            else
            {
                sequence.Append(pokemonImage.transform.DOLocalMoveX(originalPosition.x - 50, 0.25f));
            }
            sequence.Append(pokemonImage.transform.DOLocalMoveX(originalPosition.x, 0.25f));
            return sequence;
        }
        public void HitAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(pokemonImage.DOColor(Color.red, 0.1f));
            sequence.Append(pokemonImage.DOColor(Color.white, 0.1f));
        }
    }
}