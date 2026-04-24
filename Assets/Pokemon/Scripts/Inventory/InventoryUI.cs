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
            Observer.Instance.Register(EventId.OnItemUsed, InventoryUI_OnItemUsed);
        }
        void OnDisable()
        {
            ClearSelectedItem();
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
                useBtn.gameObject.SetActive(true);
                useBtn.onClick.RemoveAllListeners();
                useBtn.onClick.AddListener(() =>
                {
                    inventoryScreen.OpenPartyScreen();
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
                if (obj is PokemonPartyItemSlot target)
                {
                    bool canUse = inventory.UseItem(selectedItem, target.PokemonUnit);
                    LoadPage(currentPageIndex);
                    if (canUse)
                    {
                        float hpFraction = (float)target.PokemonUnit.HP / target.PokemonUnit.MaxHP;
                        StartCoroutine(UpdateHpBar(target, hpFraction));
                    }
                    else
                    {
                        inventoryScreen.ClosePartyScreen();
                    }
                }
            }

        }
        public IEnumerator UpdateHpBar(PokemonModal modal, float hpFraction)
        {
            yield return modal.UpdateHP(hpFraction, 0.5f);
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