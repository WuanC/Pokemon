using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI;
using UnityEngine;

namespace Pokemon.Scripts.UI
{
    public class PartyContainer : MonoBehaviour
    {
        [SerializeField] private PokemonModal[] pokemonParties;
        [SerializeField] private Vector3 startPos;
        [SerializeField] private RectTransform partyContainer;
        public void OpenParty(float duration)
        {
            partyContainer.anchoredPosition = new Vector3(startPos.x - 500, startPos.y);
            partyContainer.DOAnchorPos(startPos, duration);
        }
        public void CloseParty(float duration)
        {
            partyContainer.DOAnchorPos(new Vector3(startPos.x - 500, startPos.y), duration);
        }
        public void SetParty(List<PokemonUnit> pokemons)
        {
            for (int i = 0; i < pokemonParties.Length; i++)
            {
                if (i < pokemons.Count)
                {
                    pokemonParties[i].gameObject.SetActive(true);
                    pokemonParties[i].InitModal(pokemons[i], true);
                }
                else
                {
                    pokemonParties[i].gameObject.SetActive(false);
                }
            }
        }
    }
}