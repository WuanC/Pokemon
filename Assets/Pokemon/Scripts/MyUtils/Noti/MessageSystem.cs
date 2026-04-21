using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils.Noti
{
    public class MessageSystem : MonoBehaviour
    {
        [SerializeField] GameObject messageText;
        [SerializeField] Transform messsageParent;
        private void OnEnable()
        {
            Observer.Instance.Register(EventId.OnShowMessage, ShowMessage);
        }
        public void ShowMessage(object obj)
        {
            string message = (string)obj;
            GameObject messageObj = MyPoolManager.Instance.GetFromPool(messageText, messsageParent);
            messageObj.transform.SetParent(transform);
            messageObj.GetComponent<MessageText>().SetText(message);
        }
        private void OnDisable()
        {
            Observer.Instance.Unregister(EventId.OnShowMessage, ShowMessage);
        }
    }
}