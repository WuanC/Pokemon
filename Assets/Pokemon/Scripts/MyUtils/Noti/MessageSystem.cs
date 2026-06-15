using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using TMPro;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils.Noti
{
    public class MessageSystem : MonoBehaviour
    {
        // [SerializeField] GameObject messageText;
        // [SerializeField] Transform messsageParent;



        // private void OnEnable()
        // {
        //     Observer.Instance.Register(EventId.OnShowMessage, ShowMessage);
        // }
        // public void ShowMessage(object obj)
        // {
        //     string message = (string)obj;
        //     GameObject messageObj = MyPoolManager.Instance.GetFromPool(messageText, messsageParent);
        //     messageObj.transform.SetParent(transform);
        //     messageObj.GetComponent<MessageText>().SetText(message);
        // }
        // private void OnDisable()
        // {
        //     Observer.Instance.Unregister(EventId.OnShowMessage, ShowMessage);
        // }
        [SerializeField] private RectTransform panel;
        [SerializeField] private TextMeshProUGUI messageText;

        [Header("Animation")]
        [SerializeField] private float showDuration = 2f;
        [SerializeField] private float moveDuration = 0.35f;
        [SerializeField] private float moveOffset = 200f;

        [SerializeField] private Vector2 hiddenPos;
        [SerializeField] private Vector2 shownPos;

        private Tween currentTween;
        private Coroutine currentRoutine;

        private void Awake()
        {
            shownPos = panel.anchoredPosition;
            hiddenPos = shownPos + Vector2.up * moveOffset;

            panel.anchoredPosition = hiddenPos;
        }

        private void Start()
        {
            Observer.Instance.Register(EventId.OnShowMessage, OnNotiEvent);
        }

        private void OnNotiEvent(object notiEvent)
        {
            if (notiEvent is string message)
            {
                if (!gameObject.activeInHierarchy)
                {
                    return;
                }
                Show(message);
            }
        }
        public void Show(string message)
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentTween?.Kill();
            panel.anchoredPosition = hiddenPos;
            currentRoutine = StartCoroutine(ShowRoutine(message));
        }

        private IEnumerator ShowRoutine(string message)
        {
            messageText.text = message;

            currentTween = panel.DOAnchorPos(shownPos, moveDuration)
                .SetEase(Ease.OutBack);

            yield return currentTween.WaitForCompletion();

            yield return new WaitForSeconds(showDuration);

            currentTween = panel.DOAnchorPos(hiddenPos, moveDuration)
                .SetEase(Ease.InBack);

            yield return currentTween.WaitForCompletion();

        }

        protected void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnShowMessage, OnNotiEvent);
            currentTween?.Kill();
        }

    }
}