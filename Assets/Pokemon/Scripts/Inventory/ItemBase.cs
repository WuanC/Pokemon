using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{

    public class ItemBase : ScriptableObject
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public virtual bool Use(PokemonUnit pokemon)
        {
            return false;
        }
    }


}