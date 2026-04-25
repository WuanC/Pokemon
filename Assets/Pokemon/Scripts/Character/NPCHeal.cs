using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Character
{
    public class NPCHeal : NPCBase
    {
        public void Heal(List<PokemonUnit> party)
        {
            foreach (var pokemon in party)
            {
                pokemon.Heal();
            }
        }
    }
}