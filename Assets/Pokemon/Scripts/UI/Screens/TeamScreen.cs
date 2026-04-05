using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class TeamScreen : MonoBehaviour
    {
        [SerializeField] private Pokemon.Party party;
        [SerializeField] DragablePokemonModal[] pokemonModals;
        [SerializeField] DragablePokemonModal[] inventoryModals;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Button leftBtn;
        [field: SerializeField] public PokemonModal dummyModal { get; private set; }
        private int currentPage = 1;
        private DragablePokemonModal draggingSource;
        [SerializeField] private PokemonDetailScreen detailScreen;

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
                    pokemonModals[i].InitModal(party.PokemonParties[i]);
                    pokemonModals[i].SetupContext(this, true, i);
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
                    int inventoryIndex = i + (page - 1) * inventoryModals.Length;
                    inventoryModals[i].gameObject.SetActive(true);
                    inventoryModals[i].InitModal(party.inventory[inventoryIndex]);
                    inventoryModals[i].SetupContext(this, false, inventoryIndex);
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

        public void BeginDrag(DragablePokemonModal source)
        {
            draggingSource = source;
        }

        public void EndDrag(DragablePokemonModal source)
        {
            if (draggingSource == source)
            {
                draggingSource = null;
            }
        }

        public void DropOnTarget(DragablePokemonModal target)
        {
            if (draggingSource == null || target == null || target == draggingSource)
            {
                return;
            }

            SwapByLocation(draggingSource, target);
            SetupParty();
            SetupInventory(currentPage);
        }

        private void SwapByLocation(DragablePokemonModal source, DragablePokemonModal target)
        {
            if (source.IsPartySlot)
            {
                if (target.IsPartySlot)
                {
                    SwapPartyParty(source.DataIndex, target.DataIndex);
                    return;
                }

                SwapPartyInventory(source.DataIndex, target.DataIndex);
                return;
            }

            if (target.IsPartySlot)
            {
                SwapPartyInventory(target.DataIndex, source.DataIndex);
                return;
            }

            SwapInventoryInventory(source.DataIndex, target.DataIndex);
        }

        private void SwapPartyParty(int indexA, int indexB)
        {
            if (!IsValidPartyIndex(indexA) || !IsValidPartyIndex(indexB))
            {
                return;
            }

            var temp = party.PokemonParties[indexA];
            party.PokemonParties[indexA] = party.PokemonParties[indexB];
            party.PokemonParties[indexB] = temp;
        }

        private void SwapInventoryInventory(int indexA, int indexB)
        {
            if (!IsValidInventoryIndex(indexA) || !IsValidInventoryIndex(indexB))
            {
                return;
            }

            var temp = party.inventory[indexA];
            party.inventory[indexA] = party.inventory[indexB];
            party.inventory[indexB] = temp;
        }

        private void SwapPartyInventory(int partyIndex, int inventoryIndex)
        {
            if (!IsValidPartyIndex(partyIndex) || !IsValidInventoryIndex(inventoryIndex))
            {
                return;
            }

            var temp = party.PokemonParties[partyIndex];
            party.PokemonParties[partyIndex] = party.inventory[inventoryIndex];
            party.inventory[inventoryIndex] = temp;
        }

        private bool IsValidPartyIndex(int index)
        {
            return index >= 0 && party.PokemonParties != null && index < party.PokemonParties.Count;
        }

        private bool IsValidInventoryIndex(int index)
        {
            return index >= 0 && party.inventory != null && index < party.inventory.Count;
        }
        public void EnterDetailScreen(PokemonUnit pokemonUnit)
        {
            detailScreen.gameObject.SetActive(true);
            detailScreen.Initialize(pokemonUnit);
        }
    }
}