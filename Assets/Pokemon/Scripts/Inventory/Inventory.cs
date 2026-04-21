using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<Item> items;
        public void UseItem(Item item, PokemonUnit target)
        {
            ItemBase baseItem = item.ItemBase;
            bool canUse = baseItem.Use(target);
            if (canUse)
            {
                items.Remove(item);
            }
        }
    }


}