using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/Create Pokemon Data")]
    public class PokemonData : ScriptableObject
    {
        public string pokemonName;
        public PokemonType type;
        public Sprite frontSprite;
        public Sprite backSprite;
        public Sprite icon;

        public int catchRate = 0;
        //Stats
        public int maxHP;
        public int attack;
        public int defense;
        public int speed;

        //Moves
        public List<PokemonSkill> learnableSkills;


    }

    [System.Serializable]
    public class PokemonSkill
    {

        public SkillData skillData;
        public int levelRequirement;
    }

}