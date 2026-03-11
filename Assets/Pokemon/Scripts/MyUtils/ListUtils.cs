using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public class ListUtils
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
    }
}