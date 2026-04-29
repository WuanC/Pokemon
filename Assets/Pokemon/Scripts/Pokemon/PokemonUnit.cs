using System.Collections.Generic;
using System.Linq;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Condition;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PokemonUnit
    {
        public PokemonData Data { get; private set; }
        public int Level { get; private set; }
        public List<Skill> Skills { get; private set; }
        public int HP { get; set; }
        private Dictionary<Stat, int> statBases;
        private Dictionary<Stat, int> statBoosts;
        public int CurrentExp { get; set; }
        public ConditionData Condition { get; private set; }
        public PokemonUnit(PokemonData data, int level)
        {
            this.Data = data;
            this.Level = level;
            Skills = new List<Skill>();
            HP = MaxHP;
            CurrentExp = CalculateExpYield(level);
            for (int i = 0; i < Data.learnableSkills.Count; i++)
            {
                if (Data.learnableSkills[i].levelRequirement <= Level)
                {
                    Skills.Add(new Skill(Data.learnableSkills[i].skillData));
                }
                if (i >= 4)
                {
                    break;
                }
            }
            CalculateStat();
        }
        public PokemonUnit(PokemonSaveData saveData)
        {
            this.Data = PokemonDB.GetPokemonByName(saveData.pokemonName);
            this.Level = saveData.level;
            this.CurrentExp = saveData.currentExp;
            this.HP = saveData.hp;
            this.Condition = (saveData.conditionId != ConditionId.None) ? ConditionDB.GetConditionById(saveData.conditionId) : null;
            Skills = saveData.skills.Select(s => new Skill(s)).ToList();
            CalculateStat();
        }
        public void CalculateStat()
        {
            statBases = new Dictionary<Stat, int>()
            {
                { Stat.Attack, Mathf.FloorToInt((Data.attack * Level) / 100f) + 5 },
                { Stat.Defense, Mathf.FloorToInt((Data.defense * Level) / 100f) + 5 },
                { Stat.Speed, Mathf.FloorToInt((Data.speed * Level) / 100f) + 5 },
            };
            statBoosts = new Dictionary<Stat, int>()
            {
                { Stat.Attack, 0 },
                { Stat.Defense, 0 },
                { Stat.Speed, 0 },
            };
        }
        public int CalculateExpYield(int level)
        {
            float exp = -1;
            switch (Data.growthRate)
            {
                case GrowthRate.Fast:
                    exp = Mathf.FloorToInt(4 * Mathf.Pow(level, 3) / 5f);
                    break;
                case GrowthRate.MediumFast:
                    exp = Mathf.FloorToInt(Mathf.Pow(level, 3));
                    break;
            }
            return Mathf.FloorToInt(exp);
        }
        public bool CheckLevelUp()
        {
            if (CurrentExp >= CalculateExpYield(Level + 1))
            {
                Level++;
                return true;
            }
            return false;
        }
        public void ApplyBoost(StatBoost statBoost)
        {
            this.statBoosts[statBoost.stat] = Mathf.Clamp(this.statBoosts[statBoost.stat] + statBoost.boostAmount, -6, 6);
        }
        public int GetStat(Stat stat)
        {
            int statValue = statBases[stat];

            float[] boostValues = new float[] { 1, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };
            int statBoostIndex = statBoosts[stat];
            if (statBoostIndex >= 0)
            {
                statValue = Mathf.FloorToInt(statValue * boostValues[statBoostIndex]);
            }
            else
            {
                statValue = Mathf.FloorToInt(statValue / boostValues[-statBoostIndex]);
            }
            return statValue;
        }
        public int Attack => GetStat(Stat.Attack);

        public int Defense => GetStat(Stat.Defense);
        public int Speed => GetStat(Stat.Speed);
        public int MaxHP => Mathf.FloorToInt(Data.maxHP * Level / 100f) + 10;

        public DamageDetails TakeDamage(Skill skill, PokemonUnit attacker)
        {
            float critical = Random.value < 0.0625f ? 2f : 1f;
            float type = TypeChart.GetEffectiveness(skill.Data.elementType, Data.type);
            float modifier = Random.Range(0.85f, 1f) * type * critical;
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * skill.Data.power * ((float)attacker.Attack / Defense) + 2;
            int damage = Mathf.FloorToInt(d * modifier);
            UpdateHp(-damage);
            if (HP <= 0)
            {
                HP = 0;
                return new DamageDetails(critical, type, true, skill.Data.name);
            }
            return new DamageDetails(critical, type, false, skill.Data.name);
        }
        public void UpdateHp(int amount)
        {
            HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        }
        public Skill RandomSkill()
        {
            if (Skills.Count == 0) return null;
            return Skills[Random.Range(0, Skills.Count)];
        }
        public PokemonSkill GetSkillByLevel()
        {
            return Data.learnableSkills.FirstOrDefault(s => s.levelRequirement == Level);
        }
        public void AddSkill(Skill newSkill)
        {
            if (Skills.Count >= 4) return;
            Skills.Add(newSkill);
        }
        public void ReplaceSkill(int index, Skill newSkill)
        {
            if (index < 0 || index >= Skills.Count) return;
            Skills[index] = newSkill;
        }
        public bool HasMaxSkills()
        {
            return Skills.Count == 4;
        }
        public PokemonData GetPokemonEvoluttion()
        {
            PokemonEvolution pkmEvol = Data.evolutions.FirstOrDefault(e => e.levelRequirement <= Level);
            if (pkmEvol != null)
            {
                return pkmEvol.pokemonData;
            }
            return null;
        }
        public void Evolve(PokemonData newPokemonData)
        {
            this.Data = newPokemonData;
            HP = MaxHP;
            CalculateStat();
        }
        public PokemonSaveData GetSaveData()
        {
            return new PokemonSaveData
            {
                pokemonName = Data.pokemonName,
                level = Level,
                currentExp = CurrentExp,
                hp = HP,
                skills = Skills.Select(s => s.GetSaveData()).ToList(),
                conditionId = Condition != null ? Condition.conditionId : ConditionId.None
            };
        }
        public void Heal()
        {
            HP = MaxHP;
        }
        public void SetStatusCondition(ConditionId id)
        {
            if (id == ConditionId.None)
            {
                this.Condition = null;
            }
            else
            {
                this.Condition = ConditionDB.GetConditionById(id);
            }

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
    public class PokemonSaveData
    {
        public string pokemonName;
        public int level;
        public int currentExp;
        public int hp;
        public List<SkillSaveData> skills;
        public ConditionId conditionId;
    }
}
