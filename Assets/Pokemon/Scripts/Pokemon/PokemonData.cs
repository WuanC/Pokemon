using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/Create Pokemon Data")]
    public class PokemonData : ScriptableObject
    {
        public string pokemonName;
        public string pokemonDescription;
        public PkmType type;
        [PreviewField]
        public Sprite frontSprite;
        [PreviewField]
        public Sprite backSprite;
        [PreviewField]
        public Sprite icon;

        public int baseExp;
        public GrowthRate growthRate;
        public int catchRate = 0;
        [Title("Base Stats")]
        public int maxHP;
        public int attack;
        public int defense;
        public int speed;

        //Moves
        public List<PokemonSkill> learnableSkills;
        [Space(10)]
        //Evolutions
        public List<PokemonEvolution> evolutions;

    }

    [System.Serializable]
    public class PokemonSkill
    {
        public SkillData skillData;
        public int levelRequirement;
    }
    [System.Serializable]
    public class PokemonEvolution
    {
        public PokemonData pokemonData;
        public int levelRequirement;
    }
    public enum GrowthRate
    {
        Fast,
        MediumFast,
    }

}