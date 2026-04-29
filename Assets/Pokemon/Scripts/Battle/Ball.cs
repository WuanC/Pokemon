using System.Collections;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;

namespace Pokemon.Scripts.Battle
{
    public class Ball : MonoBehaviour
    {
        private Animator animator;
        private readonly string catchAnimSuccess = "catchSuccess";
        private readonly string catchAnimFail = "catchFail";
        [SerializeField] private AnimatorController ball;
        [SerializeField] private AnimatorController masterBall;
        private Vector3 startPos;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            startPos = transform.localPosition;
            gameObject.SetActive(false);

        }
        void OnDisable()
        {
            transform.localPosition = startPos;
        }
        public IEnumerator Throw(bool isMasterBall)
        {
            animator.runtimeAnimatorController = isMasterBall ? masterBall : ball;
            Debug.Log("Throwing " + (isMasterBall ? "Master Ball" : "Ball"));
            Debug.Log("Animator Controller: " + animator.runtimeAnimatorController.name);
            gameObject.SetActive(true);
            yield return transform.DOLocalMoveX(startPos.x + 400f, 0.5f).SetEase(Ease.OutBack).WaitForCompletion();
        }
        public IEnumerator CatchSuccess()
        {
            animator.SetBool(catchAnimSuccess, true);
            yield return new WaitForSeconds(3.5f);
            animator.SetBool(catchAnimSuccess, false);
            gameObject.SetActive(false);
        }
        public IEnumerator CatchFail()
        {
            animator.SetBool(catchAnimFail, true);
            yield return new WaitForSeconds(3f);
            animator.SetBool(catchAnimFail, false);
            gameObject.SetActive(false);

        }
    }
}