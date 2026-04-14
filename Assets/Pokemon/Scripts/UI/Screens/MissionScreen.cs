using System.Collections;
using Pokemon.Scripts.MyUtils;
using TMPro;
using UnityEngine;
using System;
using Pokemon.Scripts.Saving;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;
using Pokemon.Scripts.Quest;

namespace Pokemon.Scripts.UI.Screens
{
    public class MissionScreen : ScreenBase
    {

        [SerializeField] private Transform missionParent;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private DailyMissionHub[] dailyMissionHubs;
        Coroutine countdownCoroutine;


        void OnEnable()
        {

            UpdateHubUI();

        }
        void OnDisable()
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }
        }
        public void Initialize()
        {
            base.Active();
            int secondsUntilNextUtcDay = QuestManager.Instance.GetSecondsUntilNextUtcDay();
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
                    remainingSeconds = QuestManager.Instance.GetSecondsUntilNextUtcDay();
                    QuestManager.Instance.CreateNewDailyQuests();
                }
            }
        }

        void UpdateHubUI()
        {
            for (int i = 0; i < dailyMissionHubs.Length; i++)
            {
                if (i < QuestManager.Instance.DailyQuests.Count)
                {
                    dailyMissionHubs[i].gameObject.SetActive(true);
                    dailyMissionHubs[i].InitHub(QuestManager.Instance.DailyQuests[i]);
                }
            }
        }

    }


}