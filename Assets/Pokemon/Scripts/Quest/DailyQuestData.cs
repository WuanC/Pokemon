using UnityEngine;

namespace Pokemon.Scripts.Quest
{
    [CreateAssetMenu(fileName = "DailyQuest", menuName = "Pokemon/Quest/DailyQuest")]
    public class DailyQuestData : ScriptableObject
    {
        public string questName;
        public string description;
        public int rewardMoney;
        public int countToComplete;
        public EQuest questType;
        public Objective.ObjectiveBase objective;
    }
}