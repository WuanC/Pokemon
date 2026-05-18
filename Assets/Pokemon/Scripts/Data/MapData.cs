using System;
using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Data
{

    [CreateAssetMenu(fileName = "MapData", menuName = "Pokemon/MapData")]
    public class MapData : ScriptableObject
    {
        public string hubName;
        public MapConditionData mapCondition;
        public List<PokemonMapData> pkmRates;
        public Sprite hubIcon;
        public Sprite headerMap;
        public Sprite mapBackground;
        public int bossAndQuestCount;
        public int goldReward;
        public Map.Map mapPrefab;
        void OnValidate()
        {
            float sum = 0;
            foreach (var pokemon in pkmRates)
            {
                sum += pokemon.rate;
            }
            if (sum != 100)
            {
                Debug.LogError("Pokemon rates in " + hubName + " do not sum up to 100. Current sum: " + sum);
            }
        }

    }
    [Serializable]
    public class PokemonMapData
    {
        public PokemonData pokemonData;
        public float rate;
    }
}