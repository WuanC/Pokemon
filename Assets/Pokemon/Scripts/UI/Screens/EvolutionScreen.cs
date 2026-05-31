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

        public PairPokemonEvolution(
            PokemonData currentPkmData,
            PokemonData evolutionPkmData)
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

        [SerializeField] private RectTransform pokemonRect;

        private Vector2 originalAnchoredPos;

        private void Start()
        {
            pokemonRect =
                pokemonContainer.GetComponent<RectTransform>();

            originalAnchoredPos =
                pokemonRect.anchoredPosition;
        }

        private void OnDisable()
        {
            pokemonRect.DOKill();
            pokemonImage.DOKill();
        }

        public IEnumerator Evolution(
            List<PairPokemonEvolution> pairEvolutions)
        {
            gameObject.SetActive(true);
            pokemonContainer.SetActive(true);

            for (int i = 0; i < pairEvolutions.Count; i++)
            {
                evolutionImage.gameObject.SetActive(false);

                pokemonImage.sprite =
                    pairEvolutions[i].currentPkmData.frontSprite;
                pokemonImage.SetNativeSize();

                pokemonImage.color = Color.white;

                // Spawn bên trái
                pokemonRect.anchoredPosition =
                    originalAnchoredPos + Vector2.left * 1000f;

                yield return pokemonRect
                    .DOAnchorPos(originalAnchoredPos, 0.5f)
                    .SetEase(Ease.OutCubic)
                    .WaitForCompletion();

                yield return new WaitForSeconds(0.5f);

                // Fade evolution
                yield return pokemonImage
                    .DOFade(0.2f, 0.5f)
                    .OnComplete(() =>
                    {
                        pokemonImage.sprite =
                            pairEvolutions[i]
                                .evolutionPkmData
                                .frontSprite;

                        pokemonImage.color =
                            pokemonImage.color = new Color(0, 0, 0, 0.2f);
                    })
                    .WaitForCompletion();

                evolutionImage.gameObject.SetActive(true);

                yield return new WaitForSeconds(1.5f);

                pokemonImage.color = Color.white;

                evolutionImage.gameObject.SetActive(false);

                yield return new WaitForSeconds(1f);

                // Move ra phải
                yield return pokemonRect
                    .DOAnchorPos(
                        originalAnchoredPos + Vector2.right * 1500f,
                        0.5f)
                    .SetEase(Ease.InCubic)
                    .WaitForCompletion();
            }

            pokemonContainer.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}