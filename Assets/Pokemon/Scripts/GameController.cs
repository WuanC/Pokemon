using System;
using System.Collections.Generic;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts
{
    public enum GameState
    {
        Map,
        Battle,
    }
    public class GameController : MonoBehaviour
    {

        public Camera loungeCamera;
        public Camera battleCamera;
        private GameState currentState = GameState.Map;
        [SerializeField] private Map.Map map;
        [SerializeField] private BattleController battleController;
        Node currentNode;
        void Start()
        {
            Observer.Instance.Register(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Register(EventId.OnEndBattle, OnEndBattle);
        }
        public void OnEncounterPokemon(object data)
        {
            if (data is Tuple<Pokemon.Party, Node> tuple)
            {
                Pokemon.Party party = tuple.Item1;
                Node node = tuple.Item2;

                PokemonUnit wildPokemon = node.OwnerArea.GetRandomPokemon();
                currentNode = node;
                loungeCamera.gameObject.SetActive(false);
                battleCamera.gameObject.SetActive(true);
                currentState = GameState.Battle;
                battleController.StartBattleWithWildPokemon(party, wildPokemon);

            }

        }
        public void OnEndBattle(object data)
        {
            if (data is bool isWin)
            {
                if (isWin)
                {
                    currentNode.NodeCompleted();
                    Debug.Log("You win the battle!");
                }
                else
                {
                    Debug.Log("You lose the battle!");
                }
                loungeCamera.gameObject.SetActive(true);
                battleCamera.gameObject.SetActive(false);
                currentState = GameState.Map;
            }
        }
        void Update()
        {
            if (currentState == GameState.Map)
            {
                map.HandleInput();
            }
            else if (currentState == GameState.Battle)
            {

            }
        }
        void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Unregister(EventId.OnEndBattle, OnEndBattle);
        }


    }
}