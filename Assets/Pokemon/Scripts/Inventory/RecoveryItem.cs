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

        [Header("Condition")]
        //Condition
        public ConditionId conditionId;
        [Header("Receive")]
        public bool revive;
        public bool maxRevive;
        public override bool Use(PokemonUnit pokemon)
        {
            if (hpAmount > 0)
            {
                if (pokemon.HP == pokemon.MaxHP || pokemon.HP == 0) return false;
                pokemon.UpdateHp(hpAmount);

            }
            else if (restoreFullHp)
            {
                if (pokemon.HP == pokemon.MaxHP || pokemon.HP == 0) return false;
                pokemon.HealMax();
            }
            else if (conditionId != ConditionId.None)
            {
                if (pokemon.Condition == null || pokemon.Condition.conditionId != conditionId) return false;
                pokemon.CureCondition();
            }
            else if (revive)
            {
                if (pokemon.HP > 0) return false;
                pokemon.UpdateHp(pokemon.MaxHP / 2);
            }
            else if (maxRevive)
            {
                if (pokemon.HP > 0) return false;
                pokemon.HealMax();
            }
            return true;
        }
    }
}