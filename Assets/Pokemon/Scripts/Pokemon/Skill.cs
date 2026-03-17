using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class Skill
    {
        public SkillData Data { get; private set; }
        public Skill(SkillData data)
        {
            this.Data = data;
        }
    }
}