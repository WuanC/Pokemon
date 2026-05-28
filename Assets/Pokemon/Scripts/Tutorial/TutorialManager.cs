using System.Collections;
using System.Collections.Generic;
using Pokemon.Scripts.Battle;
using Pokemon.Scripts.Condition;
using Pokemon.Scripts.Inventory;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Scene;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Pokemon.Scripts.Tutorial
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        public const string TUTORIAL_KEY = "CompletedTutorials";
        private TutorialState currentState;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera battleCamera;

        [Title("Introduce State Config")]
        public TutorialConfig introduceConfig;


        [Title("Npc talk config")]
        private bool isPlayerClick = false;
        [SerializeField] private GameObject npcTalkPanel;
        [SerializeField] private Image npcImage;
        [SerializeField] private TextMeshProUGUI npcNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Title("Use skill")]
        [SerializeField] private GameObject battleCtrlObj;
        [SerializeField] private BattleController battleCtrl;
        [SerializeField] private Party playerParty;
        [SerializeField] private PokemonParty enemyPkm;
        [SerializeField] private Sprite campBg;
        [SerializeField] private RectTransform hand;
        private RectTransform handCache;
        [SerializeField] private Button skillBtn;
        public int btnIndex;

        [Title("Chose Pokemon")]

        [SerializeField] private TutorialConfig chosePkmConfig;
        [SerializeField] private TutorialConfig completeChosePkmConfig;
        [SerializeField] private ChosePokemonScreen chosePokemonScreen;
        [SerializeField] private PokemonParty[] chosePkmParties;


        public void Start()
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(PokemonDB.Init());
            StartCoroutine(SkillDB.Init());
            StartCoroutine(ConditionDB.Init());
            StartCoroutine(ItemDB.Init());
            currentState = TutorialState.Introduce;
            UpdateTutorialState();
        }
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPlayerClick = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Break();
            }
        }
        public static bool IsTutorialCompleted()
        {
            bool completedTutorials = PlayerPrefs.GetInt(TUTORIAL_KEY, 0) == 1;
            Debug.Log("IsTutorialCompleted: " + completedTutorials);
            return completedTutorials;
        }
        public static void MarkTutorialCompleted()
        {
            PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
            PlayerPrefs.Save();
        }
        public void UpdateTutorialState()
        {
            switch (currentState)
            {
                case TutorialState.Introduce:
                    Introduce();
                    break;
                case TutorialState.UseSkill:
                    EnterBattle();
                    break;
                case TutorialState.ChosePokemon:
                    StartCoroutine(IntroduceChosePokemon());
                    break;
                default:
                    break;
            }
        }
        public void AdvanceStep(TutorialState completedState, bool isPaused = false)
        {

            if (isPaused == false && currentState == completedState)
            {

                currentState++;
                UpdateTutorialState();
            }
        }
        public void CompleteTutorial(TutorialState state)
        {
            if (currentState == state)
            {
                AdvanceStep(state);
            }
        }

        #region State Introduce

        public void Introduce()
        {
            StartCoroutine(IntroduceCoroutine());
        }
        public IEnumerator IntroduceCoroutine()
        {
            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in introduceConfig.talkConfigs)
            {
                isPlayerClick = false;
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                dialogueText.text = talkConfig.dialogue;
                yield return new WaitUntil(() => isPlayerClick);
            }
            CompleteTutorial(TutorialState.Introduce);
        }
        #endregion

        #region  State Use Skill
        public void EnterBattle()
        {
            mainCamera.gameObject.SetActive(false);
            battleCamera.gameObject.SetActive(true);
            battleCtrlObj.SetActive(true);
            playerParty.Initialize();
            PokemonUnit enemyPkmUnit = new PokemonUnit(enemyPkm.pokemonData, enemyPkm.level);
            battleCtrl.StartBattleWithWildPokemon(playerParty, enemyPkmUnit, null, campBg);
            handCache = Instantiate(hand, skillBtn.transform);
            handCache.anchoredPosition = new Vector2(0, 100);
            skillBtn.onClick.AddListener(DestroyHand);

        }

        public void EndBattle()
        {
            battleCtrlObj.SetActive(false);
            mainCamera.gameObject.SetActive(true);


            CompleteTutorial(TutorialState.UseSkill);
        }
        public void DestroyHand()
        {
            if (handCache != null)
            {
                Destroy(handCache.gameObject);
                skillBtn.onClick.RemoveListener(DestroyHand);
                btnIndex = -1;
            }
        }
        #endregion
        #region State Chose Pokemon
        public IEnumerator IntroduceChosePokemon()
        {
            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in chosePkmConfig.talkConfigs)
            {
                isPlayerClick = false;
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                dialogueText.text = talkConfig.dialogue;
                yield return new WaitUntil(() => isPlayerClick);
            }
            npcTalkPanel.SetActive(false);
            ChosePokemon();
        }
        public void ChosePokemon()
        {

            chosePokemonScreen.gameObject.SetActive(true);
            chosePokemonScreen.InitScreen(chosePkmParties);
        }
        public void CompleteChosePokemon()
        {
            StartCoroutine(CompleteChosePokemonCoroutine());
        }
        public IEnumerator CompleteChosePokemonCoroutine()
        {
            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in completeChosePkmConfig.talkConfigs)
            {
                isPlayerClick = false;
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                dialogueText.text = talkConfig.dialogue;
                yield return new WaitUntil(() => isPlayerClick);
            }
            npcTalkPanel.SetActive(false);
            CompleteTutorial(TutorialState.ChosePokemon);
            Loader.LoadScene(Loader.Scene.GameScene);
        }
        #endregion

    }
}