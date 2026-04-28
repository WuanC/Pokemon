using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.FReward
{
    public class RewardSlot : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardQuantity;

        public void Initialize(Sprite image, int quantity)
        {
            rewardImage.sprite = image;
            rewardImage.SetNativeSize();
            rewardQuantity.text = quantity.ToString();
        }
    }
}