using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PairPokemonEvolution
    {
        public PokemonData currentPkmData;
        public PokemonData evolutionPkmData;
        public PairPokemonEvolution(PokemonData currentPkmData, PokemonData evolutionPkmData)
        {
            this.currentPkmData = currentPkmData;
            this.evolutionPkmData = evolutionPkmData;
        }
    }
    public class EvolutionScreen : MonoBehaviour
    {
        [SerializeField] private GameObject pokemonContainer;
        [SerializeField] private Image pokemonImage;
        [SerializeField] private Image evolutionImage;
        Vector3 pokemonContainerOriginalPos;
        void Start()
        {
            pokemonContainerOriginalPos = pokemonContainer.transform.position;
        }
        void OnDisable()
        {
            pokemonContainer.transform.DOKill();
            pokemonImage.DOKill();
        }
        public void Init()
        {
            pokemonContainer.SetActive(false);
        }
        public IEnumerator Evolution(List<PairPokemonEvolution> pairEvolutions)
        {
            gameObject.SetActive(true);
            pokemonContainer.SetActive(true);
            for (int i = 0; i < pairEvolutions.Count; i++)
            {
                evolutionImage.gameObject.SetActive(false);
                pokemonImage.sprite = pairEvolutions[i].currentPkmData.frontSprite;
                pokemonContainer.transform.position = pokemonContainerOriginalPos - Vector3.right * 10;
                yield return pokemonContainer.transform.DOMove(pokemonContainerOriginalPos, 0.5f).WaitForCompletion();
                yield return new WaitForSeconds(0.5f);
                yield return pokemonImage.DOFade(0.2f, 0.5f).OnComplete(() =>
                {
                    pokemonImage.sprite = pairEvolutions[i].evolutionPkmData.frontSprite;
                    pokemonImage.color = new Color(0, 0, 0, 0.2f);
                });
                evolutionImage.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                pokemonImage.color = Color.white;
                evolutionImage.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
                yield return pokemonContainer.transform.DOMove(pokemonContainerOriginalPos + Vector3.right * 15, 0.5f).WaitForCompletion();
            }
            pokemonContainer.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}