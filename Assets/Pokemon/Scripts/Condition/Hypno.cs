using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Condition
{
    [CreateAssetMenu(fileName = "HypnoCondition", menuName = "Pokemon/Create Condition/Hypno")]
    public class Hypno : ConditionData
    {

        public override bool CanApplyStatusBefore()
        {
            bool canMove = Random.Range(1, 5) == 1;
            if (canMove)
                return false;
            return true;
        }

        public override IEnumerator StatusAnimation(GameObject pokemon, float duration = 0.5F)
        {
            Debug.Log("Hypno animation");
            Observer.Instance.Broadcast(EventId.OnShowMessage, "Pokemon is hypnotized and can't move!");
            yield return pokemon.transform.DOShakePosition(duration, new Vector3(10f, 10f)).SetEase(Ease.Linear).WaitForCompletion();
        }
    }

}