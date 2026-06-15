using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils.Noti
{
    public class MessageText : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] TextMeshProUGUI text;

        public TextMeshProUGUI Text => text;
        public RectTransform Panel => panel;
    }
}