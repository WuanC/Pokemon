using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Quest.Objective;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Quest
{
    public class QuestManager : Singleton<QuestManager>, ISavable
    {
        private static string saveKey = "MissionScreen";
        private List<Quest> quests = new List<Quest>();
        public List<Quest> DailyQuests => quests;
        public void Initialize()
        {
            MissionScreenSaveData saveData = RestoreState() as MissionScreenSaveData;
            if (saveData == null || saveData.saveTime.Date < DateTime.UtcNow.Date)
            {
                GetNewDailyQuests();
                CaptureState();
            }
            else
            {
                CreateDailyQuests(saveData);
            }
        }
        void OnDestroy()
        {
            CaptureState();
        }
        public int GetSecondsUntilNextUtcDay()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime nextUtcDay = utcNow.Date.AddDays(1);
            return Mathf.Max(0, Mathf.CeilToInt((float)(nextUtcDay - utcNow).TotalSeconds));
        }
        public void GetNewDailyQuests(int count = 3)
        {
            List<DailyQuestData> allQuests = QuestDB.GetAllQuests();
            allQuests = GeneralUtils.ShuffleList(allQuests);
            int minQuest = Mathf.Min(count, allQuests.Count);
            quests.Clear();
            for (int i = 0; i < minQuest; i++)
            {
                quests.Add(new Quest(allQuests[i], 0, false));
            }
        }
        public void CaptureState()
        {
            if (quests == null || quests.Count == 0) return;
            List<QuestSaveData> saveDataList = new List<QuestSaveData>();
            foreach (var quest in quests)
            {
                saveDataList.Add(quest.GetSaveData());
            }
            MissionScreenSaveData saveData = new MissionScreenSaveData
            {
                quests = saveDataList,
                saveTime = System.DateTime.UtcNow
            };
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            PlayerPrefs.SetString(saveKey, json);
        }
        public object RestoreState()
        {
            string saveDataJson = PlayerPrefs.GetString(saveKey);
            if (string.IsNullOrEmpty(saveDataJson)) return null;

            MissionScreenSaveData saveData = JsonConvert.DeserializeObject<MissionScreenSaveData>(saveDataJson);
            return saveData;
        }
        public void CreateDailyQuests(MissionScreenSaveData saveData)
        {
            quests.Clear();
            foreach (var questSaveData in saveData.quests)
            {
                Quest quest = new Quest(questSaveData);
                quests.Add(quest);
            }
        }


        #region Process Quest Progress
        public void UpdateBattleQuestProgress(EQuest questType, PkmType pokemonType)
        {
            MissionScreenSaveData saveData = RestoreState() as MissionScreenSaveData;
            if (saveData == null || saveData.saveTime.Date < DateTime.UtcNow.Date)
            {
                GetNewDailyQuests();
                CaptureState();
            }
            foreach (var quest in quests)
            {
                if (quest.QuestData.questType == questType)
                {
                    if (quest.QuestData.objective is TypeObjective objectiveBattle)
                    {
                        if (objectiveBattle.pkmType == pokemonType)
                        {
                            quest.UpdateQuest();
                        }
                    }
                }
            }
        }

        #endregion
    }
    public class MissionScreenSaveData
    {
        public List<QuestSaveData> quests;
        public DateTime saveTime;
    }
}