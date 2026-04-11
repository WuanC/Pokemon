using UnityEngine;

namespace Pokemon.Scripts.Quest
{
    public class Quest
    {
        private DailyQuestData questData;
        private int currentCount;
        private bool isClaimed;

        public DailyQuestData QuestData => questData;
        public int CurrentCount => currentCount;
        public bool IsClaimed => isClaimed;
        public Quest(DailyQuestData questData, int currentCount, bool isClaimed)
        {
            this.questData = questData;
            this.currentCount = currentCount;
            this.isClaimed = isClaimed;
        }
        public Quest(QuestSaveData saveData)
        {
            this.questData = QuestDB.GetQuestByName(saveData.questName);
            this.currentCount = saveData.currentCount;
            this.isClaimed = saveData.isClaimed;
        }
        public QuestSaveData GetSaveData()
        {
            return new QuestSaveData
            {
                questName = questData.questName,
                currentCount = currentCount,
                isClaimed = isClaimed,
            };
        }
    }
}