using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public static class HubSaveLoad
    {
        public static void SaveBoss(string hubName, int bossAndQuestCount)
        {
            PlayerPrefs.SetInt(hubName + "_BossAndQuestCount", bossAndQuestCount);
        }
        public static int LoadBoss(string hubName)
        {
            return PlayerPrefs.GetInt(hubName + "_BossAndQuestCount", 0);
        }
    }
}