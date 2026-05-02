using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public enum PkmType
    {
        Fire,
        Water,
        Electric,
        Earth,
        Dark,
        Spirit,
        Diamond,
        Gold,
        Normal

    }
    public class TypeChart
    {
        private static float[][] chart =
        {
           //        Fire, Water,Electric,Earth, Dark, Spirit, Diamond,   Gold 
        new float[] {1f,   0.5f,   1f,     2f,    1f,   2f,   0.5f,       1f}, // Fire
        new float[] {2f,   1f,     1f,     2f,    1f,   1f,   0.5f,       1f}, // Water
        new float[] {1f,   2f,     1f,     0f,    1f,   1f,   1f,         1f},   // Electric
        new float[] {0.5f, 1f,     2f,     1f,    2f,   1f,   2f,         1f}, // Grass
        new float[] {2f,   0.5f,   0.5f,   2f,    1f,   2f,   1f,        0.5f}, // Dark
        new float[] {1f,   1f,     1f,     1f,    0.5f, 1,    2f,         1f}, // Spirit
        new float[] {2f,   1f,     1f,     0.5f,  2f,   0.5f, 1f,        1f}, // Diamond
        new float[] {1f,   1f,     1f,     1f,    1f,   1f,   1f,         1f}, // Gold
        new float[] {1f,   1f,     1f,     1f,    1f,   1f,   1f,         1f}, // Normal
};
        public static float GetEffectiveness(PkmType attackType, PkmType defenseType)
        {
            return chart[(int)attackType][(int)defenseType];
        }
        public static List<PkmType> GetStrongType(PkmType pkmType)
        {
            List<PkmType> strongTypes = new List<PkmType>();
            for (int i = 0; i < chart.Length; i++)
            {
                if (chart[(int)pkmType][i] < 1f)
                {
                    strongTypes.Add((PkmType)i);
                }
            }
            return strongTypes;
        }
        public static List<PkmType> GetWeakType(PkmType pkmType)
        {
            List<PkmType> weakTypes = new List<PkmType>();
            for (int i = 0; i < chart.Length; i++)
            {
                if (chart[(int)pkmType][i] > 1f)
                {
                    weakTypes.Add((PkmType)i);
                }
            }
            return weakTypes;
        }
    }
}