using System;
using System.Collections;
using System.Collections.Generic;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Quest;
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

        void Start()
        {
            StartCoroutine(InitGame());
            Observer.Instance.Register(EventId.OnEncounterPokemon, OnEncounterPokemon);
            Observer.Instance.Register(EventId.OnEncounterTrainer, OnEncounterTrainer);
            Observer.Instance.Register(EventId.OnEndBattle, OnEndBattle);
        }
        public IEnumerator InitGame()
        {
            yield return QuestDB.Init();
            yield return PokemonDB.Init();
            yield return SkillDB.Init();
            QuestManager.Instance.Initialize();
            HubController.Instance.Initialize();
            CurrencyManager.Instance.Initialize();
            playerParty.Initialize();

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
                if (node.Npc is NPCBattle npcBattle)
                {
                    Party party = playerParty;
                    ScreenManager.Instance.EnterBattleClick(() =>
                    {
                        currentNode = node;
                        loungeCamera.gameObject.SetActive(false);
                        battleCamera.gameObject.SetActive(true);
                        currentState = GameState.Battle;
                        battleController.StartBattleWithNPC(party, npcBattle);
                    }, npcBattle);
                }
                else if (node.Npc is NPCHeal npcHeal)
                {
                    npcHeal.Heal(playerParty.PokemonParties);
                }



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
                            pokemon.Evolve(pokemonEvolData);
                        }
                    }
                    if (pairEvolutions.Count > 0)
                    {
                        StartCoroutine(ScreenManager.Instance.Evolution(pairEvolutions));
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
        public void MapRegister(DragMap map)
        {
            this.dragMap = map;
            ScreenManager.Instance.EnterDetailMap();
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