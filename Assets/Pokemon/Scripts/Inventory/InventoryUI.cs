using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Button arrowLeft;
        [SerializeField] private Button arrowRight;
        [SerializeField] private List<ItemSlotUI> itemSlots;

        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button useBtn;
        [SerializeField] private Image useBtnIcon;
        int currentPageIndex;
        Item selectedItem;

        void Start()
        {
            arrowLeft.onClick.AddListener(() =>
            {
                LoadPage(currentPageIndex - 1);
            });
            arrowRight.onClick.AddListener(() =>
            {
                LoadPage(currentPageIndex + 1);
            });
        }
        void OnEnable()
        {
            Observer.Instance.Register(EventId.OnItemUsed, InventoryUI_OnItemUsed);
        }
        void OnDisable()
        {
            useBtn.gameObject.SetActive(false);
            descriptionText.text = "";
            selectedItem = null;
            Observer.Instance.Unregister(EventId.OnItemUsed, InventoryUI_OnItemUsed);
        }
        void OnDestroy()
        {
            arrowLeft.onClick.RemoveAllListeners();
            arrowRight.onClick.RemoveAllListeners();
            useBtn.onClick.RemoveAllListeners();
        }
        public void LoadPage(int pageIndex = 0)
        {
            if (!CanLoadPage(pageIndex)) return;
            currentPageIndex = pageIndex;
            for (int i = 0; i < itemSlots.Count; i++)
            {
                int itemIndex = pageIndex * itemSlots.Count + i;
                if (itemIndex < inventory.items.Count)
                {
                    itemSlots[i].SetItem(inventory.items[itemIndex]);
                }
                else
                {
                    itemSlots[i].SetItem(null);
                }
            }
            arrowLeft.gameObject.SetActive(CanLoadPage(pageIndex - 1));
            arrowRight.gameObject.SetActive(CanLoadPage(pageIndex + 1));
        }

        public bool CanLoadPage(int pageIndex)
        {
            if (pageIndex < 0) return false;
            return pageIndex * itemSlots.Count < inventory.items.Count;
        }
        public void SelectItem(Item item)
        {
            selectedItem = item;
            if (item == null)
            {

                useBtn.gameObject.SetActive(false);
                descriptionText.text = "";

            }
            else
            {
                useBtn.gameObject.SetActive(true);
                useBtn.onClick.RemoveAllListeners();
                useBtn.onClick.AddListener(() =>
                {
                    GetComponentInParent<InventoryScreen>().OpenPartyScreen();
                });
                descriptionText.text = item.ItemBase.description;
                useBtnIcon.sprite = item.ItemBase.icon;
                useBtnIcon.SetNativeSize();
            }
        }
        public void InventoryUI_OnItemUsed(object obj)
        {
            if (selectedItem != null)
            {
                if (obj is PokemonUnit target)
                {
                    Debug.Log($"Using {selectedItem.ItemBase.itemName} on {target.Data.pokemonName}");
                    inventory.UseItem(selectedItem, target);
                }
            }
            GetComponentInParent<InventoryScreen>().ClosePartyScreen();
        }

    }
}