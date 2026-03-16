using System.Collections.Generic;
using UnityEngine;
using Pokemon.Scripts;
using Pokemon.Scripts.Pokemon;

namespace Pokemon.Scripts.Battle
{
    public class BattleController : MonoBehaviour
    {
        public List<Pokemon.Pokemon> playerTeam;
        public List<Pokemon.Pokemon> opponentTeam;

        public void UseSkill(SkillData skill, Pokemon.Pokemon pokemonCaster, Pokemon.Pokemon pokemonTarget)
        {

        }

        public void Attack(SkillData skill, Pokemon.Pokemon pokemonCaster, Pokemon.Pokemon pokemonTarget)
        {
            if (pokemonTarget == null)
            {
                pokemonTarget = opponentTeam[0]; // For simplicity, always target the first opponent
            }
            int damage = skill.power + (int)pokemonCaster.CurrentDamage - (int)pokemonTarget.CurrentDamage;
            pokemonTarget.TakeDamage(damage);
        }

    }
}