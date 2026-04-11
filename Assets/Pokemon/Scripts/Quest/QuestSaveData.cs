using System;
using UnityEngine;

namespace Pokemon.Scripts.Quest
{
    [System.Serializable]
    public class QuestSaveData
    {
        public string questName;
        public int currentCount;
        public bool isClaimed;
    }
}