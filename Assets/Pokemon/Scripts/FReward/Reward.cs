
using System;
using System.Collections.Generic;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.FReward
{
    [Serializable]
    public class Reward
    {
        private const string ITEM_COINS = "Coins";
        private const string ITEM_STARDUST = "Dusts";
        public List<Item> items;
        public List<PokemonParty> pokemons;

        public static Reward DefaultReward(int coinQuantity, int dustQuantity)
        {
            Reward reward = new Reward();
            reward.items = new List<Item>();
            if (coinQuantity > 0) reward.items.Add(new Item(ItemDB.GetItemByName(ITEM_COINS), coinQuantity));
            if (dustQuantity > 0) reward.items.Add(new Item(ItemDB.GetItemByName(ITEM_STARDUST), dustQuantity));
            return reward;
        }

    }
}