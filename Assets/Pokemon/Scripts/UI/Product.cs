using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class Product : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI productName;
        [SerializeField] Image productIcon;
        [SerializeField] TextMeshProUGUI productPrice;
        [SerializeField] Button buyBtn;
        private Item item;


        public void SetProduct(Item item, ShopScreen shopScreen)
        {
            this.item = item;
            productName.text = item.ItemBase.itemName;
            productIcon.sprite = item.ItemBase.icon;
            productIcon.SetNativeSize();
            productIcon.rectTransform.sizeDelta = new Vector2(productIcon.rectTransform.sizeDelta.x * 1.5f, productIcon.rectTransform.sizeDelta.y * 1.5f);
            productPrice.text = item.ItemBase.price.ToString();
            buyBtn.onClick.AddListener(() =>
            {
                if (Inventory.Inventory.Instance.CanPayCoins(item.ItemBase.price))
                {
                    Inventory.Inventory.Instance.PayCoins(item.ItemBase.price);
                    Inventory.Inventory.Instance.AddItem(item);
                    shopScreen.BuySuccess(item);
                    Observer.Instance.Broadcast(EventId.OnShowMessage, "Item purchased successfully");
                }
                else
                {
                    Observer.Instance.Broadcast(EventId.OnShowMessage, "Not enough coins");
                }
            });
        }
    }
}