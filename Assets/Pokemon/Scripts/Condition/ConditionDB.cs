using System.Collections;
using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Condition
{
    public class ConditionDB : MonoBehaviour
    {
        static Dictionary<ConditionId, ConditionData> conditionDictionary;
        public static IEnumerator Init()
        {
            conditionDictionary = new Dictionary<ConditionId, ConditionData>();
            var request = Resources.LoadAll<ConditionData>("Conditions");
            yield return request;
            foreach (var condition in request)
            {
                if (conditionDictionary.ContainsKey(condition.conditionId))
                {
                    Debug.LogWarning($"Duplicate condition found: {condition.conditionId}. Skipping.");
                    continue;
                }
                conditionDictionary.Add(condition.conditionId, condition);
            }
        }
        public static ConditionData GetConditionById(ConditionId conditionId)
        {
            if (conditionDictionary.TryGetValue(conditionId, out var conditionData))
            {
                return conditionData;
            }
            Debug.LogError($"Condition not found: {conditionId}");
            return null;
        }
        public static List<ConditionData> GetAllConditions()
        {
            return new List<ConditionData>(conditionDictionary.Values);
        }
    }
}