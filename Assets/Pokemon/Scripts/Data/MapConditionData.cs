using UnityEngine;

namespace Pokemon.Scripts.Data
{
    public enum MapConditionType
    {
        None,
        UnlockMap,
        Pay,
    }
    [CreateAssetMenu(fileName = "MapConditionData", menuName = "Pokemon/MapConditionData")]
    public class MapConditionData : ScriptableObject
    {
        public MapConditionType conditionType;
        //For UnlockMap
        public MapData requiredMap;
        public float progressRequired;

        //For Pay
        public int goldRequired;

        public string description;
    }
}