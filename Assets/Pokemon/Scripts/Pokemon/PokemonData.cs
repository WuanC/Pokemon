using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/Create Pokemon Data")]
    public class PokemonData : ScriptableObject
    {
        public string pokemonName;
        public ElementType type;
        public Sprite sprite;
        public float evolutionMultiplier;
        public List<PokemonSkill> skills;
        [Header("Stats Level 1")]
        public float hp;
        public float damage;
    }

    [System.Serializable]
    public class PokemonSkill
    {
        public SkillData skillData;
        public int levelRequirement;
    }
}