using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.Condition;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.FReward;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.Map;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Quest;
using Pokemon.Scripts.Tutorial;
using Pokemon.Scripts.UI.Screens;
using Sirenix.OdinInspector;
using UnityEngine;
using Pokemon.Scripts.AudioManager;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        [SerializeField] private Party playerParty;
        [SerializeField] private BattleController battleController;
        public TypeData typeData;
        Node currentNode;
        public GameState CurrentState => currentState;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject mainCanvas;
        public Hub enableHub;
        [SerializeField]

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
            yield return ConditionDB.Init();
            yield return ItemDB.Init();
            QuestManager.Instance.Initialize();
            HubController.Instance.Initialize();

            playerParty.Initialize();

            Inventory.Inventory.Instance.Initialize();
            ScreenManager.Instance.Initialize();
            CheckForTutorial();
        }
        public void CheckForTutorial()
        {
            if (!TutorialManager.IsTutorialCompleted())
            {
                hand.gameObject.SetActive(true);
            }
        }
        public void CompleteTutorial()
        {
            hand.gameObject.SetActive(false);
            TutorialManager.MarkTutorialCompleted();
        }

#if UNITY_EDITOR
        [Button("Export Skill Learnable Report")]
        public void ExportSkillLearnableReport()
        {
            string outputPath = EditorUtility.SaveFilePanel(
                "Export skill learnable report",
                Application.dataPath,
                "skill_learnable_report",
                "csv");

            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            List<SkillData> skills = Resources.LoadAll<SkillData>("Skills")
                .OrderByDescending(skill => skill.power)
                .ThenBy(skill => skill.skillName)
                .ToList();
            List<PokemonData> pokemonDatas = Resources.LoadAll<PokemonData>("Pokemons").ToList();

            Dictionary<string, List<string>> pokemonNamesBySkill = new Dictionary<string, List<string>>();
            foreach (PokemonData pokemonData in pokemonDatas)
            {
                if (pokemonData == null || pokemonData.learnableSkills == null)
                {
                    continue;
                }

                foreach (PokemonSkill pokemonSkill in pokemonData.learnableSkills)
                {
                    if (pokemonSkill?.skillData == null || string.IsNullOrEmpty(pokemonSkill.skillData.skillName))
                    {
                        continue;
                    }

                    if (!pokemonNamesBySkill.TryGetValue(pokemonSkill.skillData.skillName, out List<string> pokemonNames))
                    {
                        pokemonNames = new List<string>();
                        pokemonNamesBySkill[pokemonSkill.skillData.skillName] = pokemonNames;
                    }

                    if (!pokemonNames.Contains(pokemonData.pokemonName))
                    {
                        pokemonNames.Add(pokemonData.pokemonName);
                    }
                }
            }

            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Skill Name,Power,Pokemon Count,Pokemon Names");

            foreach (SkillData skillData in skills)
            {
                pokemonNamesBySkill.TryGetValue(skillData.skillName, out List<string> pokemonNames);
                int pokemonCount = pokemonNames != null ? pokemonNames.Count : 0;
                string pokemonNameText = pokemonNames != null && pokemonNames.Count > 0
                    ? string.Join("; ", pokemonNames.OrderBy(name => name))
                    : string.Empty;

                csvBuilder.AppendLine($"{EscapeCsv(skillData.skillName)},{skillData.power},{pokemonCount},{EscapeCsv(pokemonNameText)}");
            }

            File.WriteAllText(outputPath, csvBuilder.ToString(), new UTF8Encoding(true));
            Debug.Log($"Exported skill report to {outputPath}");
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r') || value.Contains(';'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }
#endif
        public void OnEncounterPokemon(object data)
        {
            if (data is Node node)
            {
                Party party = playerParty;
                if (party.GetHealthyPokemon() == null)
                {
                    Observer.Instance.Broadcast(EventId.OnShowMessage, "You have no healthy Pokemon to fight!");
                    return;
                }
                MapData mapData = dragMap.GetComponent<Map.Map>().MapData;
                PokemonUnit wildPokemon = node.OwnerArea.GetRandomPokemon();
                currentNode = node;
                loungeCamera.gameObject.SetActive(false);
                battleCamera.gameObject.SetActive(true);
                currentState = GameState.Battle;
                int coinQuantity = UnityEngine.Random.Range(1, 5) * wildPokemon.Level;
                int dustQuantity = UnityEngine.Random.Range(1, 5) * wildPokemon.Level;
                battleController.StartBattleWithWildPokemon(party, wildPokemon, Reward.DefaultReward(coinQuantity, dustQuantity), mapData.mapBackground);
                AudioService.Instance.PlayMusic(AudioId.BGM_BattleStart, () =>
                {
                    AudioService.Instance.PlayMusic(AudioId.BGM_BattleLoop);
                });
            }
        }
        public void OnEncounterTrainer(object data)
        {
            if (data is Node node)
            {
                if (node.Npc is NPCBattle npcBattle)
                {
                    Party party = playerParty;
                    if (party.GetHealthyPokemon() == null)
                    {
                        Observer.Instance.Broadcast(EventId.OnShowMessage, "You have no healthy Pokemon to fight!");
                        return;
                    }
                    MapData mapData = dragMap.GetComponent<Map.Map>().MapData;
                    ScreenManager.Instance.EnterBattleClick(() =>
                    {
                        AudioService.Instance.PlayMusic(AudioId.BGM_BattleStart, () =>
                        {
                            AudioService.Instance.PlayMusic(AudioId.BGM_BattleLoop);
                        });
                        currentNode = node;
                        loungeCamera.gameObject.SetActive(false);
                        battleCamera.gameObject.SetActive(true);
                        currentState = GameState.Battle;
                        battleController.StartBattleWithNPC(party, npcBattle, npcBattle.reward, mapData.mapBackground);
                    }, npcBattle);
                }
                else if (node.Npc is NPCHeal npcHeal)
                {
                    npcHeal.EnterNpc(playerParty.PokemonParties);
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
                            PlayerParty.Instance.AddPkmToDex(pokemon);
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
                AudioService.Instance.PlayMusic(AudioId.BGM_Hub);
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