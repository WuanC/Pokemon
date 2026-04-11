using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

namespace Pokemon.Scripts.Quest
{
    public class QuestDB
    {
        static Dictionary<string, DailyQuestData> questDictionary;
        public static IEnumerator Init()
        {
            questDictionary = new Dictionary<string, DailyQuestData>();
            var request = Resources.LoadAll<DailyQuestData>("Quests");
            yield return request;
            foreach (var quest in request)
            {
                if (questDictionary.ContainsKey(quest.questName))
                {
                    Debug.LogWarning($"Duplicate quest name found: {quest.questName}. Skipping.");
                    continue;
                }
                questDictionary.Add(quest.questName, quest);
            }
        }
        public static DailyQuestData GetQuestByName(string questName)
        {
            if (questDictionary.TryGetValue(questName, out var questData))
            {
                return questData;
            }
            Debug.LogError($"Quest not found: {questName}");
            return null;
        }
        public static List<DailyQuestData> GetAllQuests()
        {
            return new List<DailyQuestData>(questDictionary.Values);
        }
    }
}