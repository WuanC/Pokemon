
using System;
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
        public PokemonUnit Pokemon { get; private set; }
        void Start()
        {
            originalPosition = pokemonImage.transform.localPosition;
        }
        public void SetPokemon(PokemonUnit pokemon)
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
            EnterAniamtion();
        }


        public void EnterAniamtion()
        {
            if (isPlayerPokemon)
            {
                pokemonImage.transform.localPosition = new Vector3(-1000, originalPosition.y);
            }
            else
            {
                pokemonImage.transform.localPosition = new Vector3(1000, originalPosition.y);
            }
            pokemonImage.transform.DOLocalMoveX(originalPosition.x, 0.75f);
        }
        public void AttackAnimation()
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
        }
        public void HitAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(pokemonImage.DOColor(Color.red, 0.1f));
            sequence.Append(pokemonImage.DOColor(Color.white, 0.1f));
        }
        public void FaintAnimation(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(pokemonImage.transform.DOLocalMoveY(originalPosition.y - 150, 0.5f));
            sequence.Join(pokemonImage.DOFade(0, 0.5f));
            sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
                Debug.Log("Faint");
                pokemonImage.color = new Color(pokemonImage.color.r, pokemonImage.color.g, pokemonImage.color.b, 1);
            });
        }
    }
}