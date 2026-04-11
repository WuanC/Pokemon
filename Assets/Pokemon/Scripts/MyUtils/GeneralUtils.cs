using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public class GeneralUtils
    {
        public static List<T> ShuffleList<T>(List<T> list)
        {
            List<T> newList = new(list);

            for (int i = 0; i < newList.Count; i++)
            {
                int randIndex = UnityEngine.Random.Range(0, newList.Count);
                (newList[i], newList[randIndex]) = (newList[randIndex], newList[i]);
            }

            return newList;
        }
        public static string FormatTime(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }
    }
}