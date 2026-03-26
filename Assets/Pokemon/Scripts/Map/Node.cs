using System;
using DG.Tweening;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public bool HasPokemon;
        public GameObject markBattle;
        public Color disableColor;
        public bool IsLock = true;
        public Action OnNodeCompleted;
        public Vector3 startMarkPosition;
        public Area OwnerArea { get; private set; }

        public void InitializeNode(Area area)
        {

            {
                OwnerArea = area;
                startMarkPosition = markBattle.transform.position;
                SetDisable(true);
                SetHasPokemon(false);
            }
        }
        public void SetHasPokemon(bool state)
        {
            HasPokemon = state;
            if (state)
            {
                markBattle.SetActive(true);
                markBattle.transform.DOMoveY(startMarkPosition.y + 0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else
            {
                markBattle.transform.position = startMarkPosition;
                markBattle.SetActive(false);
                markBattle.transform.DOKill();
            }
        }
        public void SetDisable(bool state)
        {
            if (state)
            {
                spriteRenderer.color = disableColor;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        public void Unlock()
        {
            IsLock = false;
            SetDisable(false);
        }
        public void NodeCompleted()
        {
            OnNodeCompleted?.Invoke();
        }
        void OnDestroy()
        {
            markBattle.transform.DOKill();
        }


    }
}