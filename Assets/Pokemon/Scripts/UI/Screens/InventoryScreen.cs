using Pokemon.Scripts.Inventory;
using UnityEngine;

namespace Pokemon.Scripts.UI.Screens
{
    public class InventoryScreen : ScreenBase
    {
        [SerializeField] private InventoryUI inventoryUI;
        public void Initialize()
        {
            base.Active();
            inventoryUI.LoadPage(0);
        }
    }
}