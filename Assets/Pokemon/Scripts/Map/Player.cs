using DG.Tweening;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator animator;
        private const string MOVING_ANIMATION_KEY = "isMoving";
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
                target.NodeCompleted();
                animator.SetBool(MOVING_ANIMATION_KEY, false);
            });
        }
    }
}