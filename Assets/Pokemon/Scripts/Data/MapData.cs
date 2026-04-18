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
        public List<PokemonData> pokemonInMaps;
        public Sprite hubIcon;
        public Sprite headerMap;
        public Sprite mapBackground;
        public int bossAndQuestCount;
        public int goldReward;
        public Map.Map mapPrefab;

    }
}