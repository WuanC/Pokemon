
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonPartyItemSlot : PokemonModal, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Observer.Instance.Broadcast(EventId.OnSelectPKMUseItem, this);
        }
    }
}