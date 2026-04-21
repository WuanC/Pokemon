using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils.Noti
{
    public class MessageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        public void SetText(string message)
        {
            text.text = message;
            transform.localPosition = Vector2.zero;
            RectTransform itemRect = gameObject.GetComponent<RectTransform>();
            transform.DOLocalMoveY(transform.localPosition.y + 200, 2f).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
        }
        void OnDisable()
        {
            transform.DOKill();
        }
    }
}