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
        [SerializeField] private Pokemon.Party playerParty;
        [SerializeField] private BattleController battleController;
        Node currentNode;
        [SerializeField] private SplashScreen splashScreen;
        [SerializeField] private EnterHubScreen enterHubScreen;
        [SerializeField] private EnterBattleScreen enterBattleScreen;
        [SerializeField] private PlayScreen playScreen;
        [SerializeField] private EvolutionScreen evolutionScreen;
        void Start()
        {
            Observer.Instance.Register(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Register(EventId.OnEncounterTrainer, OnEncounterTrainer);
            Observer.Instance.Register(EventId.OnEndBattle, OnEndBattle);
        }
        public void OnEncounterPokemon(object data)
        {
            if (data is Node node)
            {
                Pokemon.Party party = playerParty;

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
            if (data is Node node)
            {
                Pokemon.Party party = playerParty;
                NPC npc = node.Npc;
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
                    List<PairPokemonEvolution> pairEvolutions = new List<PairPokemonEvolution>();
                    foreach (var pokemon in playerParty.PokemonParties)
                    {
                        if (pokemon.HP <= 0) continue;
                        var pokemonEvolData = pokemon.GetPokemonEvoluttion();
                        if (pokemonEvolData != null)
                        {
                            pairEvolutions.Add(new PairPokemonEvolution(pokemon.Data, pokemonEvolData));
                        }
                    }
                    if (pairEvolutions.Count > 0)
                    {
                        StartCoroutine(evolutionScreen.Evolution(pairEvolutions));
                    }
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
        public void EnterHubClick(Action goBtnAction, Hub hub)
        {
            enterHubScreen.Initialize(goBtnAction, hub);
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
            playScreen.EnterDetailMap();
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