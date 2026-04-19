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
        public Skill(SkillSaveData saveData)
        {
            this.Data = SkillDB.GetSkillByName(saveData.skillName);
        }
        public SkillSaveData GetSaveData()
        {
            return new SkillSaveData()
            {
                skillName = Data.skillName
            };
        }
    }
    public class SkillSaveData
    {
        public string skillName;
    }
}