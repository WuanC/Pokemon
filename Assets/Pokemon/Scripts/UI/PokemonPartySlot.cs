
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonPartySlot : PokemonModal, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (pokemonUnit == null || pokemonUnit.HP <= 0)
            {
                Debug.Log("Cannot select fainted or empty slot!");
                return;
            }
            Observer.Instance.Broadcast(EventId.OnSwitchPokemon, this);
        }
    }
}