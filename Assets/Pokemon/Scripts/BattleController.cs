using System.Collections.Generic;
using UnityEngine;
using Pokemon.Scripts;
using Pokemon.Scripts.Pokemon;

namespace Pokemon.Scripts
{
    public class BattleController : MonoBehaviour
    {
        public List<PokemonBattle> playerTeam;
        public List<PokemonBattle> opponentTeam;

        public void UseSkill(SkillData skill, PokemonBattle pokemonCaster, PokemonBattle pokemonTarget)
        {

        }

        public void Attack(SkillData skill, PokemonBattle pokemonCaster, PokemonBattle pokemonTarget)
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