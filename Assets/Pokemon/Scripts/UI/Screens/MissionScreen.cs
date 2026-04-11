using System.Collections;
using Pokemon.Scripts.MyUtils;
using TMPro;
using UnityEngine;
using System;
using Pokemon.Scripts.Saving;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;

namespace Pokemon.Scripts.UI.Screens
{
    public class MissionScreen : ScreenBase, ISavable
    {
        private static string saveKey = "MissionScreen";
        [SerializeField] private Transform missionParent;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private DailyMissionHub[] dailyMissionHubs;
        Coroutine countdownCoroutine;

        private List<Quest.Quest> quests = new List<Quest.Quest>();
        void OnEnable()
        {
            if (!RestoreState())
            {
                GetNewDailyQuests();
            }
            else
            {
                UpdateHubUI();
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetNewDailyQuests();
            }
        }
        void OnDisable()
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                CaptureState();
            }
        }
        public void Initialize()
        {
            base.Active();
            int secondsUntilNextUtcDay = GetSecondsUntilNextUtcDay();
            countdownCoroutine = StartCoroutine(SetCountdown(secondsUntilNextUtcDay));
        }
        public IEnumerator SetCountdown(int remainingSeconds)
        {
            while (true)
            {
                countdownText.text = GeneralUtils.FormatTime(remainingSeconds);
                yield return new WaitForSeconds(1f);
                remainingSeconds--;
                if (remainingSeconds <= 0)
                {
                    remainingSeconds = GetSecondsUntilNextUtcDay();
                    GetNewDailyQuests();
                }
            }
        }
        private int GetSecondsUntilNextUtcDay()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime nextUtcDay = utcNow.Date.AddDays(1);
            return Mathf.Max(0, Mathf.CeilToInt((float)(nextUtcDay - utcNow).TotalSeconds));
        }
        public void GetNewDailyQuests(int count = 3)
        {
            List<Quest.DailyQuestData> allQuests = Quest.QuestDB.GetAllQuests();
            allQuests = GeneralUtils.ShuffleList(allQuests);
            int minQuest = Mathf.Min(count, allQuests.Count);
            quests.Clear();
            for (int i = 0; i < minQuest; i++)
            {
                quests.Add(new Quest.Quest(allQuests[i], 0, false));
            }
            UpdateHubUI();
        }
        void UpdateHubUI()
        {
            for (int i = 0; i < dailyMissionHubs.Length; i++)
            {
                if (i < quests.Count)
                {
                    dailyMissionHubs[i].gameObject.SetActive(true);
                    dailyMissionHubs[i].InitHub(quests[i]);
                }
            }
        }
        public void CaptureState()
        {
            if (quests == null || quests.Count == 0) return;
            List<Quest.QuestSaveData> saveDataList = new List<Quest.QuestSaveData>();
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
            //Debug.Log(json);
            PlayerPrefs.SetString(saveKey, json);
        }
        public bool RestoreState()
        {
            string saveDataJson = PlayerPrefs.GetString(saveKey);
            if (string.IsNullOrEmpty(saveDataJson)) return false;

            MissionScreenSaveData saveData = JsonConvert.DeserializeObject<MissionScreenSaveData>(saveDataJson);
            if (saveData == null)
            {
                return false;
            }

            if (saveData.saveTime.Date < System.DateTime.UtcNow.Date)
            {
                return false;
            }
            quests.Clear();
            foreach (var questSaveData in saveData.quests)
            {
                Quest.Quest quest = new Quest.Quest(questSaveData);
                quests.Add(quest);
            }
            return true;
        }
    }

    public class MissionScreenSaveData
    {
        public List<Quest.QuestSaveData> quests;
        public DateTime saveTime;
    }
}