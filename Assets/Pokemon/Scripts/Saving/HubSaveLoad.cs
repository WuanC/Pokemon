using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public static class HubSaveLoad
    {
        public static void SaveBossAndQuest(string hubName, int bossAndQuestCount)
        {
            PlayerPrefs.SetInt(hubName + "_BossAndQuestCount", bossAndQuestCount);
        }
        public static int LoadBossAndQuest(string hubName)
        {
            return PlayerPrefs.GetInt(hubName + "_BossAndQuestCount", 0);
        }
        public static void SavePokemonCount(string hubName, int pokemonCount)
        {
            PlayerPrefs.SetInt(hubName + "_PokemonCount", pokemonCount);
        }
        public static int LoadPokemonCount(string hubName)
        {
            return PlayerPrefs.GetInt(hubName + "_PokemonCount", 0);
        }
    }
}