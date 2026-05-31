using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator animator;
        public List<Node> allNodesInMap = new();
        Sequence sequence;
        private const string MOVING_ANIMATION_KEY = "isMoving";

        public void MoveToTarget(Node target)
        {
            if (sequence != null && sequence.IsActive())
            {
                sequence.Kill();
            }
            if (target == null || moveSpeed <= 0f)
            {
                return;
            }

            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform, false);
            }

            Node startNode = FindNearestNode(transform.position);
            if (startNode == null)
            {
                return;
            }

            List<Node> path = FindPathBfs(startNode, target);
            if (path.Count == 0)
            {
                return;
            }

            animator.SetBool(MOVING_ANIMATION_KEY, true);

            sequence = DOTween.Sequence();
            Vector3 currentPos = transform.position;

            foreach (Node pathNode in path)
            {
                if (pathNode == null)
                {
                    continue;
                }

                Vector3 destination = pathNode.transform.position;
                float distance = Vector3.Distance(currentPos, destination);
                if (distance <= Mathf.Epsilon)
                {
                    continue;
                }

                float segmentDuration = distance / moveSpeed;
                sequence.Append(transform.DOMove(destination, segmentDuration).SetEase(Ease.Linear));
                currentPos = destination;
            }

            sequence.OnComplete(() =>
            {
                if (target.nodeState == NodeState.HasPokemon)
                {
                    Observer.Instance.Broadcast(EventId.OnEncounterPokemon, target);
                }
                else if (target.nodeState == NodeState.HasBattleTrainer || target.nodeState == NodeState.HasOtherTrainer)
                {
                    Observer.Instance.Broadcast(EventId.OnEncounterTrainer, target);
                }

                animator.SetBool(MOVING_ANIMATION_KEY, false);
            });
        }

        private Node FindNearestNode(Vector3 position)
        {
            Node nearestNode = null;
            float minDistanceSqr = float.MaxValue;

            foreach (Node node in allNodesInMap)
            {
                if (node == null || node.IsLock)
                {
                    continue;
                }

                float distanceSqr = (node.transform.position - position).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearestNode = node;
                }
            }

            return nearestNode;
        }

        private List<Node> FindPathBfs(Node start, Node target)
        {
            List<Node> emptyPath = new();
            if (start == null || target == null)
            {
                return emptyPath;
            }

            if (start == target)
            {
                return new List<Node> { target };
            }

            Queue<Node> queue = new();
            HashSet<Node> visited = new();
            Dictionary<Node, Node> parent = new();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                foreach (Node next in current.ConnectedNodes)
                {
                    if (next == null || visited.Contains(next))
                    {
                        continue;
                    }

                    if (next.IsLock && next != target)
                    {
                        continue;
                    }

                    visited.Add(next);
                    parent[next] = current;

                    if (next == target)
                    {
                        return ReconstructPath(start, target, parent);
                    }

                    queue.Enqueue(next);
                }
            }

            return emptyPath;
        }

        private List<Node> ReconstructPath(Node start, Node target, Dictionary<Node, Node> parent)
        {
            List<Node> path = new();
            Node current = target;

            while (current != null)
            {
                path.Add(current);
                if (current == start)
                {
                    break;
                }

                parent.TryGetValue(current, out current);
            }

            path.Reverse();
            return path;
        }

        void OnDestroy()
        {
            transform.DOKill();
        }
    }
}