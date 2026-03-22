using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [System.Serializable]
    public class StatBoost
    {
        public Stat stat;
        public int boostAmount;
    }
    public enum Stat
    {
        Attack,
        Defense,
        Speed,
        Accuracy

    }
}