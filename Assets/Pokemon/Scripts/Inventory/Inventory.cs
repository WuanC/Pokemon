using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    public class Inventory : Singleton<Inventory>, ISavable
    {
        [SerializeField] private string saveKey = "Inventory";
        public List<Item> items { get; private set; }
        void OnDestroy()
        {
            CaptureState();
        }
        public void Initialize()
        {
            List<ItemSaveData> saveData = RestoreState() as List<ItemSaveData>;
            if (saveData != null)
            {
                items = new List<Item>();
                foreach (var itemData in saveData)
                {
                    items.Add(new Item(itemData));
                }
            }
            else
            {
                items = new List<Item>();
            }
        }
        public bool UseItem(Item item, PokemonUnit target)
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
            return canUse;
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
            Observer.Instance.Broadcast(EventId.OnUpdateItem, item);
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
            Observer.Instance.Broadcast(EventId.OnUpdateItem, item);
        }
        public Item GetCoins()
        {
            return items.FirstOrDefault(i => i.ItemBase.itemName == "Coins");
        }
        public Item GetDusts()
        {
            return items.FirstOrDefault(i => i.ItemBase.itemName == "Dusts");
        }
        public static Item InitCoins(int quantity)
        {
            return new Item(ItemDB.GetItemByName("Coins"), quantity);
        }
        public static Item InitDusts(int quantity)
        {
            return new Item(ItemDB.GetItemByName("Dusts"), quantity);
        }

        public void CaptureState()
        {
            Debug.Log("Capture Inventory State");
            if (items == null || items.Count == 0) return;
            List<ItemSaveData> saveDataList = new List<ItemSaveData>();
            foreach (var item in items)
            {
                saveDataList.Add(item.GetSaveData());
            }
            string json = JsonConvert.SerializeObject(saveDataList, Formatting.Indented);
            PlayerPrefs.SetString(saveKey, json);
        }

        public object RestoreState()
        {
            string saveDataJson = PlayerPrefs.GetString(saveKey);
            if (string.IsNullOrEmpty(saveDataJson)) return null;

            return JsonConvert.DeserializeObject<List<ItemSaveData>>(saveDataJson);
        }

    }


}