using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    [System.Serializable]
    public class Area
    {
        public Node keyNode;
        public List<Node> nodes;
        public List<PokemonData> pokemonsInArea;
        public Vector2Int rangeLevel;
        public void InitializeArena(bool isUnlock = true)
        {
            if (keyNode != null && !isUnlock)
            {
                keyNode.OnNodeCompleted += OnKeyNodeCompleted;
            }
            for (int j = 0; j < this.nodes.Count; j++)
            {
                this.nodes[j].InitializeNode(this);
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
            List<Node> nodes = ListUtils.ShuffleList(this.nodes);
            for (int j = 0; j < nodes.Count; j++)
            {
                if (j < random)
                {
                    nodes[j].SetHasPokemon(true);
                }
                else
                {
                    nodes[j].SetHasPokemon(false);
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
            PokemonData data = pokemonsInArea[Random.Range(0, pokemonsInArea.Count)];
            return new PokemonUnit(data, level);
        }
    }
}