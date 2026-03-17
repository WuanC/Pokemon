using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public enum PokemonType
    {
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Dark,
        Spirit,
    }
    public class TypeChart
    {
        private static float[][] chart =
        {
    //N   F    W    E    G    D    S
    new float[] {1f, 1f, 1f, 1f, 1f, 1f, 1f}, // Normal
    new float[] {1f, 0.5f, 0.5f, 1f, 2f, 1f, 1f}, // Fire
    new float[] {1f, 2f, 0.5f, 1f, 0.5f, 1f, 1f}, // Water
    new float[] {1f, 1f, 2f, 0.5f, 0.5f, 1f, 1f}, // Electric
    new float[] {1f, 0.5f, 2f, 1f, 0.5f, 1f, 1f}, // Grass
    new float[] {1f, 1f, 1f, 1f, 1f, 0.5f, 2f}, // Dark
    new float[] {1f, 1f, 1f, 1f, 1f, 2f, 0.5f}, // Spirit
};
        public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
        {
            return chart[(int)attackType][(int)defenseType];
        }
    }
}