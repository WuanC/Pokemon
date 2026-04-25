using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;

namespace Pokemon.Scripts.Character
{
    public class NPCHeal : NPCBase
    {
        public void EnterNpc(List<PokemonUnit> party)
        {
            ScreenManager.Instance.EnterHealScreen(party, this);
        }
        public void Heal(List<PokemonUnit> party)
        {
            foreach (var pokemon in party)
            {
                pokemon.Heal();
            }
        }
    }
}