using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    public class ItemDB
    {
        static Dictionary<string, ItemBase> itemDictionary;
        public static IEnumerator Init()
        {
            itemDictionary = new Dictionary<string, ItemBase>();
            var request = Resources.LoadAll<ItemBase>("Items");
            yield return request;
            foreach (var item in request)
            {
                if (itemDictionary.ContainsKey(item.itemName))
                {
                    Debug.LogWarning($"Duplicate item name found: {item.itemName}. Skipping.");
                    continue;
                }
                itemDictionary.Add(item.itemName, item);
            }
        }
        public static ItemBase GetItemByName(string itemName)
        {
            if (itemDictionary.TryGetValue(itemName, out var itemData))
            {
                return itemData;
            }
            Debug.LogError($"Item not found: {itemName}");
            return null;
        }
        public static List<ItemBase> GetAllItems()
        {
            return new List<ItemBase>(itemDictionary.Values);
        }
    }
}