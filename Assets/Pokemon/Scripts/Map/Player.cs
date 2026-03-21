using System;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator animator;
        private Pokemon.Party party;
        private const string MOVING_ANIMATION_KEY = "isMoving";
        void Awake()
        {
            party = GetComponent<Pokemon.Party>();
        }
        public void MoveToTarget(Node target)
        {

            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform, false);
            }
            else
            {
                animator.SetBool(MOVING_ANIMATION_KEY, true);
            }
            float moveDuration = Vector3.Distance(transform.position, target.transform.position) / moveSpeed;
            transform.DOMove(target.transform.position, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (target.HasPokemon)
                {
                    Observer.Instance.Broadcast(EventId.OnEncounterPokemon, Tuple.Create(party, target));
                }
                animator.SetBool(MOVING_ANIMATION_KEY, false);
            });
        }
    }
}