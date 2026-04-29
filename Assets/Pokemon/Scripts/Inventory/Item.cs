using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private ItemBase itemBase;
        [SerializeField] private int quantity;
        public ItemBase ItemBase => itemBase;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
            }
        }
        public Item(ItemBase itemBase, int quantity)
        {
            this.itemBase = itemBase;
            this.quantity = quantity;
        }
        public Item(ItemSaveData saveData)
        {
            this.itemBase = ItemDB.GetItemByName(saveData.itemName);
            this.quantity = saveData.quantity;
        }
        public ItemSaveData GetSaveData()
        {

            return new ItemSaveData
            {

                itemName = itemBase.itemName,
                quantity = quantity
            };
        }
    }
    public class ItemSaveData
    {
        public string itemName;
        public int quantity;
    }
}