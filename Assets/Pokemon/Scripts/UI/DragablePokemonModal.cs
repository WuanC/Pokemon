using System;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon.Scripts.UI
{
    public class DragablePokemonModal : PokemonModal, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        private TeamScreen teamScreen;
        private CanvasGroup canvasGroup;
        private bool isDragging;
        public bool IsPartySlot { get; private set; }
        public int DataIndex { get; private set; } = -1;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetupContext(TeamScreen teamScreen, bool isPartySlot, int dataIndex)
        {
            this.teamScreen = teamScreen;
            IsPartySlot = isPartySlot;
            DataIndex = dataIndex;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (teamScreen == null || pokemonUnit == null || DataIndex < 0)
            {
                return;
            }

            isDragging = true;
            teamScreen.BeginDrag(this);
            teamScreen.dummyModal.InitModal(pokemonUnit);
            teamScreen.dummyModal.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            teamScreen.dummyModal.gameObject.SetActive(true);

            // Let pointer raycast pass through this source while dragging.
            canvasGroup.alpha = 0.25f;
            canvasGroup.blocksRaycasts = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (teamScreen == null || teamScreen.dummyModal == null || !teamScreen.dummyModal.gameObject.activeSelf)
            {
                return;
            }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            teamScreen.dummyModal.transform.position = mousePos;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (teamScreen == null)
            {
                return;
            }

            teamScreen.DropOnTarget(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (teamScreen != null)
            {
                teamScreen.EndDrag(this);
            }

            if (teamScreen != null && teamScreen.dummyModal != null)
            {
                teamScreen.dummyModal.gameObject.SetActive(false);
            }

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            isDragging = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isDragging || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            teamScreen.EnterDetailScreen(pokemonUnit);

        }
    }
}