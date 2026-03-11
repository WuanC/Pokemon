using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Pokemon/Create Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public ElementType elementType;

        public SkillType type;

        public int power;

        public SkillEffect effect;

        public float effectChance;
    }
}