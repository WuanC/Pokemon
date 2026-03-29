using System;
using System.Collections.Generic;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;

namespace Pokemon.Scripts
{
    public enum GameState
    {
        Map,
        Battle,
    }
    public class GameController : Singleton<GameController>
    {

        public GameObject loungeCamera;
        public GameObject battleCamera;
        private GameState currentState = GameState.Map;
        public DragMap dragWorld;
        private DragMap dragMap;
        [SerializeField] private BattleController battleController;
        Node currentNode;
        [SerializeField] private SplashScreen splashScreen;
        [SerializeField] private EnterHubScreen enterHubScreen;
        [SerializeField] private EnterBattleScreen enterBattleScreen;
        [SerializeField] private PlayScreen playSceen;
        void Start()
        {
            Observer.Instance.Register(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Register(EventId.OnEncounterTrainer, OnEncounterTrainer);
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
        public void OnEncounterTrainer(object data)
        {
            if (data is Tuple<Pokemon.Party, Node> tuple)
            {
                Pokemon.Party party = tuple.Item1;
                Node node = tuple.Item2;
                EnterBattleClick(() =>
                {
                    currentNode = node;
                    loungeCamera.gameObject.SetActive(false);
                    battleCamera.gameObject.SetActive(true);
                    currentState = GameState.Battle;
                    battleController.StartBattleWithNPC(party, node.Npc);
                }, node.Npc);


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
                if (dragMap == null)
                {
                    dragWorld.HandleInput();
                }
                else
                {
                    dragMap.HandleInput();
                }
            }
            else if (currentState == GameState.Battle)
            {

            }
        }
        public void EnterHubClick(Action goBtnAction)
        {
            enterHubScreen.Initialize(goBtnAction);
        }
        public void EnterBattleClick(Action onFightBtnClick, NPC npc)
        {
            enterBattleScreen.Initialize(onFightBtnClick, npc);
        }
        public void ActiveSplashScreen(Action onComplete)
        {
            splashScreen.onFadeComplete = onComplete;
            splashScreen.Fade();
        }
        public void MapRegister(DragMap map)
        {
            this.dragMap = map;
            playSceen.EnterDetailMap();
            dragWorld.gameObject.SetActive(false);
        }
        public void BackToWorldMap()
        {
            if (dragMap != null)
            {
                dragWorld.gameObject.SetActive(true);
                Destroy(dragMap.gameObject);
                dragMap = null;
            }
        }
        void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Unregister(EventId.OnEncounterTrainer, OnEncounterTrainer);
            Observer.Instance.Unregister(EventId.OnEndBattle, OnEndBattle);
        }


    }
}