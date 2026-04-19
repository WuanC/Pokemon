using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class Area : MonoBehaviour
    {
        public Node keyNode;
        public List<Node> nodes;
        public Vector2Int rangeLevel;
        private Map map;
        public int arenaIndex;
        void Awake()
        {
            nodes = new List<Node>(GetComponentsInChildren<Node>());
        }
        public void InitializeArena(Map map, int index, bool isUnlock = true)
        {

            this.map = map;
            this.arenaIndex = index;
            if (keyNode != null && !isUnlock)
            {
                keyNode.OnNodeCompleted += OnKeyNodeCompleted;
            }
            for (int j = 0; j < this.nodes.Count; j++)
            {
                this.nodes[j].InitializeNode(this, map.HubName, j);
                this.nodes[j].OnNodeCompleted += RandomPokemonInArea;
            }
        }
        private void OnKeyNodeCompleted()
        {
            UnlockArea();
            RandomPokemonInArea();

            keyNode.OnNodeCompleted -= OnKeyNodeCompleted;
        }
        public void RandomPokemonInArea()
        {
            int random = UnityEngine.Random.Range(1, 3);
            List<Node> nodes = GeneralUtils.ShuffleList(this.nodes);
            for (int j = 0; j < nodes.Count; j++)
            {
                if (j < random)
                {
                    if (nodes[j].nodeState == NodeState.HasTrainer)
                    {
                        random++;
                        continue;
                    }
                    nodes[j].SetNodeState(NodeState.HasPokemon);
                }
                else
                {
                    if (nodes[j].nodeState == NodeState.HasTrainer)
                    {
                        continue;
                    }
                    nodes[j].SetNodeState(NodeState.None);
                }
            }
        }
        public void UnlockArea()
        {
            for (int j = 0; j < this.nodes.Count; j++)
            {
                this.nodes[j].Unlock();
            }
        }
        public PokemonUnit GetRandomPokemon()
        {
            int level = UnityEngine.Random.Range(rangeLevel.x, rangeLevel.y + 1);
            PokemonData data = map.pokemonInMaps[Random.Range(0, map.pokemonInMaps.Count)];
            return new PokemonUnit(data, level);
        }
    }
}