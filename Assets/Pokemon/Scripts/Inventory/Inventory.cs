using System.Collections.Generic;
using System.Linq;
using Pokemon.Scripts.MyUtils;
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
                RemoveItem(item);
                Observer.Instance.Broadcast(EventId.OnShowMessage, $"Used {baseItem.itemName} on {target.Data.pokemonName}");
            }
            else
            {
                Observer.Instance.Broadcast(EventId.OnShowMessage, $"Can't use {baseItem.itemName} on {target.Data.pokemonName}");
            }
        }
        public void RemoveItem(Item item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                items.Remove(item);
            }
        }
        public void AddItem(Item item)
        {
            Item itemInInventory = items.FirstOrDefault(i => i.ItemBase == item.ItemBase);
            if (itemInInventory != null)
            {
                itemInInventory.Quantity += item.Quantity;
            }
            else
            {
                items.Add(item);
            }
        }
    }


}