using UnityEngine;

namespace Pokemon.Scripts.Inventory
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private ItemBase itemBase;
        [SerializeField] private int quantity;
        public ItemBase ItemBase => itemBase;
        public int Quantity => quantity;
        public Item(ItemBase itemBase, int quantity)
        {
            this.itemBase = itemBase;
            this.quantity = quantity;
        }
    }
}