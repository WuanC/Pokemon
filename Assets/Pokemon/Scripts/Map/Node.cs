using System;
using System.Collections.Generic;
using System.Linq;
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
        [Header("Graph")]
        [SerializeField] private List<Node> connectedNodes = new();

        public IReadOnlyList<Node> ConnectedNodes => connectedNodes;
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
#if UNITY_EDITOR

        private void OnValidate()
        {
            connectedNodes = connectedNodes
    .Where(x => x != null && x != this)
    .Distinct()
    .ToList();
            foreach (Node node in connectedNodes)
            {
                if (node == null)
                    continue;
                if (!node.ConnectedNodes.Contains(this))
                {
                    node.connectedNodes.Add(this);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = IsLock ? Color.gray : Color.green;
            Gizmos.DrawSphere(transform.position, 0.2f);

            foreach (var node in connectedNodes)
            {
                if (node == null)
                    continue;

                Vector3 start = transform.position;
                Vector3 end = node.transform.position;

                // Đường nối
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(start, end);

                // Mũi tên
                DrawArrow(start, end);
            }
        }

        private void DrawArrow(Vector3 start, Vector3 end)
        {
            Vector3 dir = (end - start).normalized;

            Vector3 arrowPos = Vector3.Lerp(start, end, 0.8f);

            float arrowSize = 0.25f;

            Vector3 right =
                Quaternion.Euler(0, 0, 25) * -dir * arrowSize;

            Vector3 left =
                Quaternion.Euler(0, 0, -25) * -dir * arrowSize;

            Gizmos.color = Color.red;

            Gizmos.DrawLine(arrowPos, arrowPos + right);
            Gizmos.DrawLine(arrowPos, arrowPos + left);
        }
#endif
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
                    if (TrainerSaveLoad.LoadTrainerData(NodeName) == 1)
                    {
                        npc.gameObject.SetActive(false);
                        return;
                    }
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
                markBattle.transform.localPosition = startMarkLocalPosition;
                markBattle.transform.DOLocalMoveY(startMarkLocalPosition.y + 0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else
            {
                markBattle.transform.DOKill();
                markBattle.transform.localPosition = startMarkLocalPosition;
                markBattle.SetActive(false);
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
                int currentBoss = HubSaveLoad.LoadBoss(hubName);
                HubSaveLoad.SaveBoss(hubName, currentBoss + 1);
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