using System.Collections.Generic;
using Pokemon.Scripts.Battle;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PokemonUnit
    {
        public PokemonData Data { get; private set; }
        public int Level { get; private set; }
        public List<Skill> Skills { get; private set; }
        public int HP { get; set; }
        public PokemonUnit(PokemonData data, int level)
        {
            this.Data = data;
            this.Level = level;
            Skills = new List<Skill>();
            HP = MaxHP;
            for (int i = 0; i < data.learnableSkills.Count; i++)
            {
                if (data.learnableSkills[i].levelRequirement <= level)
                {
                    Skills.Add(new Skill(data.learnableSkills[i].skillData));
                }
                if (i >= 4)
                {
                    break;
                }
            }
        }
        public int Attack => Mathf.FloorToInt((Data.attack * Level) / 100f) + 5;

        public int Defense => Mathf.FloorToInt((Data.defense * Level) / 100f) + 5;
        public int Speed => Mathf.FloorToInt((Data.speed * Level) / 100f) + 5;
        public int MaxHP => Mathf.FloorToInt((Data.maxHP * Level) / 100f) + 10;

        public DamageDetails TakeDamage(Skill skill, PokemonUnit attacker)
        {
            float critical = Random.value < 0.625f ? 2f : 1f;
            float type = TypeChart.GetEffectiveness(skill.Data.elementType, Data.type);
            float modifier = Random.Range(0.85f, 1f) * type * critical;
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * skill.Data.power * ((float)attacker.Attack / Defense) + 2;
            int damage = Mathf.FloorToInt(d * modifier);
            HP -= damage;
            if (HP < 0)
            {
                HP = 0;
                return new DamageDetails(critical, type, true, skill.Data.name);
            }
            return new DamageDetails(critical, type, false, skill.Data.name);
        }
        public Skill RandomSkill()
        {
            if (Skills.Count == 0) return null;
            return Skills[Random.Range(0, Skills.Count)];
        }
    }
    public class DamageDetails
    {
        public float critical;
        public float typeEffectiveness;
        public bool isFainted;
        public string skillName;
        public DamageDetails(float critical, float typeEffectiveness, bool isFainted, string skillName)
        {
            this.critical = critical;
            this.typeEffectiveness = typeEffectiveness;
            this.isFainted = isFainted;
            this.skillName = skillName;
        }
    }
}
