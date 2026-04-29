using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    [CreateAssetMenu(fileName = "New Catch Item", menuName = "Pokemon/Items/Catch Item")]
    public class CatchItem : ItemBase
    {
        public int catchRate;
        public bool isMasterBall;
        public override bool Use(PokemonUnit pokemon)
        {
            return true;
        }
    }
}