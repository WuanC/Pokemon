using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public static class TrainerSaveLoad
    {
        //1 means defeated, 0 means not defeated
        public static void SaveTrainerData(string nodeName)
        {
            PlayerPrefs.SetInt(nodeName, 1);
        }
        public static int LoadTrainerData(string nodeName)
        {
            return PlayerPrefs.GetInt(nodeName, 0);
        }
    }
}