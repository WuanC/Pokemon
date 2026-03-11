using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PokemonBattle : MonoBehaviour
    {
        private PokemonData data;
        [SerializeField] private int currentLevel;
        public float CurrentDamage { get; private set; }
        [SerializeField] private float currentHp;
        [SerializeField] private List<SkillData> currentSkills;
        private BattleController battleController;
        public bool IsMyPokemon; // 1 for player, 0 for opponent
        public void InitializePokemon(PokemonData data, int level)
        {
            this.data = data;
            this.currentLevel = level;
            this.CurrentDamage = data.damage * level;
            this.currentHp = data.hp * level;
        }
        public (SkillData, PokemonBattle) UseSkill(PokemonBattle pokemonCaster, PokemonBattle pokemonTarget, SkillData skill)
        {
            return (skill, pokemonTarget);
        }
        public void TakeDamage(float damage)
        {
            this.currentHp -= damage;
            if (currentHp <= 0)
            {
                currentHp = 0;
                // Handle Pokemon fainting
            }
        }
        public void UseSkill()
        {
            BattleController battleController = FindObjectOfType<BattleController>();
            battleController.Attack(currentSkills[0], this, null);
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsMyPokemon)
            {
                UseSkill();
            }
        }
    }
}