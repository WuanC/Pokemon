using System.Collections;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Condition
{
    public abstract class ConditionData : ScriptableObject
    {
        public ConditionId conditionId;
        public Sprite reportIcon;
        public virtual bool CanApplyStatusBefore()
        {
            return false;
        }
        public virtual bool CanApplyStatusAfter(PokemonUnit pokemon)
        {
            return false;
        }
        public abstract IEnumerator StatusAnimation(GameObject pokemon, float duration = 0.5f);
    }
}