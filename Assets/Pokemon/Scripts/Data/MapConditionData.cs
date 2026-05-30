using Sirenix.OdinInspector;
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
        [ShowIf("conditionType", MapConditionType.UnlockMap)]
        public MapData requiredMap;

        [ShowIf("conditionType", MapConditionType.Pay)]
        public int goldRequired;

        public string description;
    }
}