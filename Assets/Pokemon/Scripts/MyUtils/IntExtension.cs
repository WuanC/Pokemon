using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public static class IntExtension
    {
        public static int ClampAdd(int currentValue, int amount)
        {
            long result = (long)currentValue + amount;

            if (result > int.MaxValue)
                return int.MaxValue;

            if (result < int.MinValue)
                return int.MinValue;

            return (int)result;
        }
    }
}