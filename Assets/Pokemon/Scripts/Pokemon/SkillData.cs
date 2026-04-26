using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Pokemon/Create Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public string description;
        public PkmType elementType;
        public Sprite icon;
        public int power;
        public int accuracy;
        public CategorySkill category;

        public TargetType targetType;
        public int skillPriority;
        public GameObject vfxPrefab;
        public MoveEffect moveEffect;
    }
    [System.Serializable]
    public class MoveEffect
    {
        public List<StatBoost> statBoosts;
        public ConditionId conditionId;
    }
    public enum CategorySkill
    {
        Attack,
        Status,
    }
    public enum TargetType
    {
        Self,
        Enemy,
    }
}