using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "New Recovery Item", menuName = "Pokemon/Items/Recovery Item")]
    public class RecoveryItem : ItemBase
    {
        [Header("Hp")]
        public int hpAmount;
        public bool restoreFullHp;

        [Header("PP")]
        public int ppAmount;
        public bool restoreFullPp;
        //Condition

        [Header("Receive")]
        public bool revive;
        public bool maxRevive;
        public override bool Use(PokemonUnit pokemon)
        {
            if (hpAmount > 0)
            {
                if (pokemon.HP == pokemon.MaxHP) return false;
                pokemon.UpdateHp(hpAmount);

            }
            else if (restoreFullHp)
            {
                if (pokemon.HP == pokemon.MaxHP) return false;
                pokemon.UpdateHp(pokemon.MaxHP);
            }
            return true;
        }
    }
}