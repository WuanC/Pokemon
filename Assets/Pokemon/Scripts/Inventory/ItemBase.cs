using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{

    [CreateAssetMenu(fileName = "Item", menuName = "Pokemon/Items/Normal")]
    public class ItemBase : ScriptableObject
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public int price;
        public virtual bool Use(PokemonUnit pokemon)
        {
            Observer.Instance.Broadcast(EventId.OnShowMessage, $"Can't use {itemName} on {pokemon.Data.pokemonName}");
            return false;
        }
    }


}