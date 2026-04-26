using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.Condition;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Condition
{
    [CreateAssetMenu(fileName = "SickCondition", menuName = "Pokemon/Create Condition/Sick")]
    public class SickCondition : ConditionData
    {
        public override bool CanApplyStatusAfter(PokemonUnit pokemon)
        {
            pokemon.UpdateHp(-Mathf.FloorToInt(pokemon.Data.maxHP / 8f));
            return true;
        }


        public override IEnumerator StatusAnimation(GameObject pokemon, float duration = 0.5F)
        {
            yield return pokemon.transform.DOShakePosition(duration, new Vector3(10f, 10f)).SetEase(Ease.Linear).WaitForCompletion();
        }
    }
}