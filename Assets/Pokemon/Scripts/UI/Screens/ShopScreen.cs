using System.Collections.Generic;
using Pokemon.Scripts.FReward;
using Pokemon.Scripts.Inventory;
using UnityEngine;

namespace Pokemon.Scripts.UI.Screens
{
    public class ShopScreen : ScreenBase
    {
        [SerializeField] private List<Item> products;
        [SerializeField] private Product productPrefab;
        [SerializeField] private Transform productContainer;

        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private RewardSlot slot;
        protected override void Start()
        {
            base.Start();
            foreach (var item in products)
            {
                var productObj = Instantiate(productPrefab, productContainer);
                productObj.SetProduct(item, this);
            }
        }
        public void BuySuccess(Item item)
        {
            rewardPanel.SetActive(true);
            slot.Initialize(item.ItemBase.icon, item.Quantity);
        }
    }
}