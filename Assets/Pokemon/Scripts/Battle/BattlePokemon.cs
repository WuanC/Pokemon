
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class BattlePokemon : MonoBehaviour
    {
        [SerializeField] bool isPlayerPokemon;
        public Image pokemonImage;
        [SerializeField] Vector3 originalPosition;
        [SerializeField] PokemonModal pokemonHub;

        [Title("Stat Progress")]
        [SerializeField] private StatGUI statAttack;
        [SerializeField] private StatGUI statDefense;
        [SerializeField] private StatGUI statSpeed;
        public PokemonUnit Pokemon { get; private set; }
        public bool IsPlayerPokemon => isPlayerPokemon;
        public Skill CurrentSkill { get; set; }

        private void OnDisable()
        {
            pokemonImage.transform.DOKill();
            pokemonImage.transform.localPosition = originalPosition;
            pokemonImage.transform.localScale = Vector3.one;
            pokemonImage.color = Color.white;

        }
        public void SetPokemon(PokemonUnit pokemon, float duration = 0.5f)
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
            pokemonImage.SetNativeSize();
            pokemonHub.InitModal(pokemon, isPlayerPokemon);
            EnterAnimation(duration);

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
        public void ExitAnimation(float duration, Action onComplete = null)
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
        public void UpdateStatUI(StatBoost statBoosts, float duration)
        {
            StatGUI cachedStatGUI = null;
            if (statBoosts.stat == Stat.Attack)
            {
                cachedStatGUI = statAttack;
            }
            else if (statBoosts.stat == Stat.Defense)
            {
                cachedStatGUI = statDefense;

            }
            else if (statBoosts.stat == Stat.Speed)
            {
                cachedStatGUI = statSpeed;
            }
            if (cachedStatGUI != null)
            {
                cachedStatGUI.gameObject.SetActive(true);
                cachedStatGUI.SetArrowDirection(statBoosts.boostAmount > 0);
                StartCoroutine(DisableStatUI(duration, cachedStatGUI.gameObject));
            }
        }
        IEnumerator DisableStatUI(float duration, GameObject statUI)
        {
            yield return new WaitForSeconds(duration);
            statUI.SetActive(false);

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
        public IEnumerator CatchAnimation(Ball ball)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(pokemonImage.transform.DOScale(0f, 0.5f));
            sequence.Join(pokemonImage.transform.DOMove(ball.transform.position, 0.5f));
            sequence.Join(pokemonImage.DOFade(0.1f, 0.5f));
            yield return sequence.WaitForCompletion();
        }
        public IEnumerator CatchFailAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(pokemonImage.transform.DOScale(1f, 0.5f));
            sequence.Join(pokemonImage.transform.DOLocalMove(originalPosition, 0.5f));
            sequence.Join(pokemonImage.DOFade(1f, 0.5f));
            yield return sequence.WaitForCompletion();
        }
        #region Update Hub
        public void UpdateHub()
        {
            pokemonHub.UpdateModal();
        }
        public IEnumerator UpdateExpBar(bool isReset = false)
        {
            yield return pokemonHub.UpdateExpBar(isReset);
        }
        public IEnumerator UpdateHp(float hpFraction, float duration)
        {
            yield return pokemonHub.UpdateHP(hpFraction, duration);
        }
        public IEnumerator UpdateHp(float duration)
        {
            float hpFraction = (float)Pokemon.HP / Pokemon.MaxHP;
            yield return pokemonHub.UpdateHP(hpFraction, duration);
        }
        public void UpdateStatus(ConditionId conditionId)
        {
            pokemonHub.UpdateStatus(conditionId);
        }
        #endregion
    }
}