using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class ScreenBase : MonoBehaviour
    {
        [SerializeField] protected Button exitBtn;
        [SerializeField] protected GameObject unlockPannel;
        [SerializeField] protected GameObject backGround;
        protected Tween moveTween;

        protected virtual void Start()
        {
            exitBtn.onClick.AddListener(Deactive);
        }
        public virtual void Active()
        {
            gameObject.SetActive(true);
            backGround.gameObject.SetActive(true);
            unlockPannel.SetActive(true);
            moveTween?.Kill();
            backGround.transform.position = new Vector2(backGround.transform.position.x, backGround.transform.position.y + 10);
            moveTween = backGround.transform.DOMoveY(backGround.transform.position.y - 10, 0.2f).SetEase(Ease.OutQuad);
        }
        public virtual void Deactive()
        {
            moveTween?.Kill();
            moveTween = backGround.transform.DOMoveY(backGround.transform.position.y + 10, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                backGround.transform.position = new Vector2(backGround.transform.position.x, backGround.transform.position.y - 10);
                backGround.gameObject.SetActive(false);
                unlockPannel.SetActive(false);
                gameObject.SetActive(false);
            });
        }
        void OnDestroy()
        {
            exitBtn.onClick.RemoveAllListeners();
        }
    }
}