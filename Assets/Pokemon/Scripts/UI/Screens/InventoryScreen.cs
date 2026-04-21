using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class InventoryScreen : ScreenBase
    {
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private PlayerParty playerParty;

        [SerializeField] private GameObject partyGameObject;
        [SerializeField] private PartyContainer partyContainer;
        [SerializeField] private Button closePartyBtn;

        protected override void Start()
        {
            base.Start();
            closePartyBtn.onClick.AddListener(() => ClosePartyScreen());
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            closePartyBtn.onClick.RemoveAllListeners();
        }
        public void Initialize()
        {
            base.Active();
            inventoryUI.LoadPage(0);
            partyContainer.SetParty(playerParty.PokemonParties);
        }
        public void OpenPartyScreen()
        {
            partyGameObject.SetActive(true);
            partyContainer.OpenParty(0.2f);
        }
        public void ClosePartyScreen(bool isBattle = false)
        {
            partyContainer.CloseParty(0f);
            partyGameObject.SetActive(false);
            if (isBattle)
            {
                gameObject.SetActive(false);
            }
        }

    }
}