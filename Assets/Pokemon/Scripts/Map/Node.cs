using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public enum NodeState
    {
        None,
        HasPokemon,
        HasBattleTrainer,
        HasOtherTrainer,
        HasCoins,
    }
    public class Node : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("NPC Data")]
        [SerializeField] private NPCBase npc;


        public NodeState nodeState;
        public GameObject markBattle;
        public Color disableColor;
        public bool IsLock = true;
        public Action OnNodeCompleted;
        public Vector3 startMarkLocalPosition;
        public Area OwnerArea { get; private set; }
        public NPCBase Npc => npc;
        private string hubName;
        public string NodeName { get; private set; }
        void Awake()
        {
            npc = GetComponentInChildren<NPCBase>();
        }
        public void InitializeNode(Area area, string hubName, int nodeIndex)
        {
            OwnerArea = area;
            this.hubName = hubName;
            this.NodeName = $"{hubName}_Area{area.arenaIndex}_Node{nodeIndex}";
            startMarkLocalPosition = markBattle.transform.localPosition;
            SetDisable(true);
            SetNodeState(NodeState.None);
            SetupNPCData();
        }
        public void SetupNPCData()
        {
            if (npc != null)
            {
                if (npc is NPCBattle)
                {
                    if (TrainerSaveLoad.LoadTrainerData(NodeName) != 0) return;
                    npc.gameObject.SetActive(true);
                    npc.SetupNPCData();
                    SetNodeState(NodeState.HasBattleTrainer);
                }
                else
                {
                    npc.gameObject.SetActive(true);
                    npc.SetupNPCData();
                    SetNodeState(NodeState.HasOtherTrainer);
                }

            }
        }
        public void SetNodeState(NodeState state)
        {
            nodeState = state;
            if (state != NodeState.None && state != NodeState.HasOtherTrainer)
            {
                markBattle.transform.DOKill();
                markBattle.SetActive(true);
                markBattle.transform.DOLocalMoveY(startMarkLocalPosition.y + 0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else
            {
                markBattle.transform.localPosition = startMarkLocalPosition;
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
            if (nodeState == NodeState.HasBattleTrainer)
            {
                npc.gameObject.SetActive(false);
                SetNodeState(NodeState.None);
                int currentBossAndQuestCount = HubSaveLoad.LoadBossAndQuest(hubName);
                HubSaveLoad.SaveBossAndQuest(hubName, currentBossAndQuestCount + 1);
                TrainerSaveLoad.SaveTrainerData(NodeName);
            }
            OnNodeCompleted?.Invoke();
        }
        void OnDestroy()
        {
            markBattle.transform.DOKill();
        }
    }
}