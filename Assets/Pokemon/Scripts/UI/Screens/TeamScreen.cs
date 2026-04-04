using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class TeamScreen : MonoBehaviour
    {
        [SerializeField] private Pokemon.Party party;
        [SerializeField] PokemonModal[] pokemonModals;
        [SerializeField] PokemonModal[] inventoryModals;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Button leftBtn;
        private int currentPage = 1;

        void OnEnable()
        {
            SetupParty();
            SetupInventory(1);
        }
        void Start()
        {
            rightBtn.onClick.AddListener(() =>
            {
                SetupInventory(currentPage + 1);
            });
            leftBtn.onClick.AddListener(() =>
            {
                SetupInventory(currentPage - 1);
            });

        }
        void OnDestroy()
        {
            rightBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
        }
        public void SetupParty()
        {
            for (int i = 0; i < pokemonModals.Length; i++)
            {
                if (i < party.PokemonParties.Count)
                {
                    pokemonModals[i].gameObject.SetActive(true);
                    pokemonModals[i].SetModal(party.PokemonParties[i]);
                }
                else
                {
                    pokemonModals[i].gameObject.SetActive(false);
                }
            }
        }
        public void SetupInventory(int page)
        {
            if (!CanLoadPage(page)) return;
            currentPage = page;
            for (int i = 0; i < inventoryModals.Length; i++)
            {
                if (party.inventory == null)
                {
                    inventoryModals[i].gameObject.SetActive(false);
                    continue;
                }
                if (i + (page - 1) * inventoryModals.Length < party.inventory.Count)
                {
                    inventoryModals[i].gameObject.SetActive(true);
                    inventoryModals[i].SetModal(party.inventory[i + (page - 1) * inventoryModals.Length]);
                }
                else
                {
                    inventoryModals[i].gameObject.SetActive(false);
                }
            }
            rightBtn.gameObject.SetActive(CanLoadPage(page + 1));
            leftBtn.gameObject.SetActive(page > 1);
        }
        public bool CanLoadPage(int page)
        {
            if (page == 1) return true;
            if (page < 1 || party.inventory == null) return false;
            if (party.inventory.Count <= (page - 1) * inventoryModals.Length)
            {
                return false;
            }
            return true;
        }
    }
}