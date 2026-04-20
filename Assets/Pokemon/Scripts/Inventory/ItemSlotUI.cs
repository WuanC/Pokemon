using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Inventory
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Button selectBtn;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;
        private Item item;
        InventoryUI inventoryUI;
        void Awake()
        {
            inventoryUI = GetComponentInParent<InventoryUI>();
        }
        void Start()
        {
            selectBtn.onClick.AddListener(() =>
            {
                inventoryUI.SelectItem(item);
            });

        }
        public void SetItem(Item item)
        {
            this.item = item;
            if (item != null)
            {
                selectBtn.gameObject.SetActive(true);
                icon.sprite = item.ItemBase.icon;
                icon.SetNativeSize();

                countText.text = item.Quantity.ToString();

            }
            else
            {
                selectBtn.gameObject.SetActive(false);
                countText.text = "";
            }
        }
        void OnDestroy()
        {
            selectBtn.onClick.RemoveAllListeners();
        }
    }
}