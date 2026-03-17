using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Pokemon/Create Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public PokemonType elementType;
        public Sprite icon;


        public int power;
        public int accuracy;

    }
}