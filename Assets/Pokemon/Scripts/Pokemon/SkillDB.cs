using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class SkillDB
    {
        static Dictionary<string, SkillData> skillDictionary;
        public static IEnumerator Init()
        {
            skillDictionary = new Dictionary<string, SkillData>();
            var request = Resources.LoadAll<SkillData>("Skills");
            yield return request;
            foreach (var skill in request)
            {
                if (skillDictionary.ContainsKey(skill.skillName))
                {
                    Debug.LogWarning($"Duplicate skill name found: {skill.skillName}. Skipping.");
                    continue;
                }
                skillDictionary.Add(skill.skillName, skill);
            }
        }
        public static SkillData GetSkillByName(string skillName)
        {
            if (skillDictionary.TryGetValue(skillName, out var skillData))
            {
                return skillData;
            }
            Debug.LogError($"Skill not found: {skillName}");
            return null;
        }
        public static List<SkillData> GetAllSkills()
        {
            return new List<SkillData>(skillDictionary.Values);
        }
    }
}