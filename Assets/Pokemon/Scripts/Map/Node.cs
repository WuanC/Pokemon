using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Character;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public enum NodeState
    {
        None,
        HasPokemon,
        HasTrainer,
        HasCoins,
    }
    public class Node : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("NPC Data")]
        [SerializeField] private NPC npc;
        public bool IsLoseBattle { get; private set; }

        public NodeState nodeState;
        public GameObject markBattle;
        public Color disableColor;
        public bool IsLock = true;
        public Action OnNodeCompleted;
        public Vector3 startMarkPosition;
        public Area OwnerArea { get; private set; }
        public NPC Npc => npc;
        public void InitializeNode(Area area)
        {
            OwnerArea = area;
            startMarkPosition = markBattle.transform.position;
            SetDisable(true);
            SetNodeState(NodeState.None);
            SetupNPCData();
        }
        public void SetupNPCData()
        {
            if (npc != null)
            {
                npc.gameObject.SetActive(true);
                npc.SetupNPCData();
                SetNodeState(NodeState.HasTrainer);
            }
        }
        public void SetNodeState(NodeState state)
        {
            nodeState = state;
            if (state != NodeState.None)
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
            if (nodeState == NodeState.HasTrainer)
            {
                IsLoseBattle = true;
                npc.gameObject.SetActive(false);
                SetNodeState(NodeState.None);
            }
            OnNodeCompleted?.Invoke();
        }
        void OnDestroy()
        {
            markBattle.transform.DOKill();
        }


    }
}