using System.Collections;
using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI;
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
        [SerializeField] private bool isBattle;
        private InventoryScreen inventoryScreen;
        int currentPageIndex;
        Item selectedItem;

        void Awake()
        {
            inventoryScreen = GetComponentInParent<InventoryScreen>();
        }

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
            Observer.Instance.Register(EventId.OnSelectPKMUseItem, InventoryUI_OnSelectPKMUseItem);
        }
        void OnDisable()
        {
            ClearSelectedItem();
            Observer.Instance.Unregister(EventId.OnSelectPKMUseItem, InventoryUI_OnSelectPKMUseItem);
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
            ClearSelectedItem();
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
            if (pageIndex == 0) return true;
            return pageIndex * itemSlots.Count < inventory.items.Count;
        }
        public void SelectItem(Item item)
        {
            selectedItem = item;
            if (item == null)
            {
                ClearSelectedItem();
            }
            else
            {
                if (item.ItemBase is CatchItem)
                {
                    SelectCatchItem(item);
                }
                else if (item.ItemBase is RecoveryItem)
                {
                    SelectRecoveryItem(item);
                }
                else
                {
                    SelectOnlyViewItem(item);
                }
            }
        }
        public void SelectOnlyViewItem(Item item)
        {
            useBtn.gameObject.SetActive(false);
            descriptionText.text = item.ItemBase.description;
        }
        public void SelectCatchItem(Item item)
        {
            if (GameController.Instance.CurrentState == GameState.Battle)
            {
                useBtn.gameObject.SetActive(true);
                useBtn.onClick.RemoveAllListeners();
                useBtn.onClick.AddListener(() =>
                {
                    inventoryScreen.ClosePartyScreen(isBattle);
                    Observer.Instance.Broadcast(EventId.OnItemDiscatchUsed, item);
                });
                useBtnIcon.sprite = item.ItemBase.icon;
                useBtnIcon.SetNativeSize();
                descriptionText.text = item.ItemBase.description;
            }
            else
            {
                SelectOnlyViewItem(item);
            }


        }
        public void SelectRecoveryItem(Item item)
        {

            useBtn.gameObject.SetActive(true);
            useBtn.onClick.RemoveAllListeners();
            useBtn.onClick.AddListener(() =>
            {
                inventoryScreen.OpenPartyScreen();
            });
            useBtnIcon.sprite = item.ItemBase.icon;
            useBtnIcon.SetNativeSize();
            descriptionText.text = item.ItemBase.description;

        }
        public void InventoryUI_OnSelectPKMUseItem(object obj)
        {
            if (selectedItem != null)
            {
                if (obj is PokemonPartyItemSlot target)
                {
                    bool canUse = inventory.UseItem(selectedItem, target.PokemonUnit);
                    LoadPage(currentPageIndex);
                    if (canUse)
                    {
                        StartCoroutine(UpdateModal(target));
                    }
                    else
                    {
                        inventoryScreen.ClosePartyScreen();
                    }
                }
            }

        }
        public IEnumerator UpdateModal(PokemonModal modal)
        {
            modal.UpdateModal();
            yield return new WaitForSeconds(0.5f);

            inventoryScreen.ClosePartyScreen(isBattle);
            Observer.Instance.Broadcast(EventId.OnItemUsedInBattle, true);
        }
        public void ClearSelectedItem()
        {
            selectedItem = null;
            useBtn.gameObject.SetActive(false);
            descriptionText.text = "";
        }

    }
}