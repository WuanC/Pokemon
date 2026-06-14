using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.WDebug
{
    public class DebugScreen : MonoBehaviour
    {
        [SerializeField] private Button unlockAllHubBtn;
        [SerializeField] private Button unlockAllPokemonBtn;
        [SerializeField] private Button unlockAllItemsBtn;
        private void Start()
        {
            unlockAllHubBtn.onClick.AddListener(OnUnlockAllHubClicked);
            unlockAllPokemonBtn.onClick.AddListener(OnUnlockAllPokemonClicked);
            unlockAllItemsBtn.onClick.AddListener(OnUnlockAllItemsClicked);
        }
        private void OnDestroy()
        {
            unlockAllHubBtn.onClick.RemoveAllListeners();
            unlockAllPokemonBtn.onClick.RemoveAllListeners();
            unlockAllItemsBtn.onClick.RemoveAllListeners();
        }
        private void OnUnlockAllHubClicked()
        {
            HubController.Instance.UnlockAllHubs();
        }
        private void OnUnlockAllPokemonClicked()
        {
            foreach (var pkmData in PokemonDB.GetAllPokemon())
            {
                PokemonUnit pokemonUnit = new PokemonUnit(pkmData, 50);
                PlayerParty.Instance.AddPokemon(pokemonUnit);
            }
        }
        private void OnUnlockAllItemsClicked()
        {
            foreach (var itemData in ItemDB.GetAllItems())
            {
                Item item = new Item(itemData, 999);
                Inventory.Inventory.Instance.AddItem(item);
            }
        }


    }
}