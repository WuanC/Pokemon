using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private float textSpeed = 0.03f; // Thời gian delay giữa mỗi chữ (giây)

        [Title("Use skill")]
        [SerializeField] private GameObject battleCtrlObj;
        [SerializeField] private BattleController battleCtrl;
        [SerializeField] private Party playerParty;
        [SerializeField] private PokemonParty enemyPkm;
        [SerializeField] private Sprite campBg;
        [SerializeField] private Sprite catchBg;
        [SerializeField] private RectTransform hand;
        private RectTransform handCache;
        [SerializeField] private Button skillBtn;
        public int btnIndex;

        [Title("Chose Pokemon")]
        [SerializeField] private TutorialConfig chosePkmConfig;
        [SerializeField] private TutorialConfig completeChosePkmConfig;
        [SerializeField] private ChosePokemonScreen chosePokemonScreen;
        [SerializeField] private PokemonParty[] chosePkmParties;

        [Title("Reward Tutorial")]
        [SerializeField] private RewardTutorialScreen rewardTutorialScreen;
        [Title("Catch Pokemon")]
        [SerializeField] private TutorialConfig catchPkmConfig;
        [SerializeField] private TutorialConfig completeCatchPkmConfig;
        private bool isTextTweening = false;
        [SerializeField] private Button moreBtn;
        [SerializeField] private Button catchBtn;
        public Button targetBtn;

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
                case TutorialState.Reward:
                    RewardTutorial();
                    break;
                case TutorialState.CatchPokemon:
                    StartCoroutine(IntroduceCatchPokemon());
                    break;
                default:
                    Loader.LoadScene(Loader.Scene.GameScene);
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

        // HÀM TỰ VIẾT: Hiệu ứng đánh máy bằng Coroutine thuần Unity
        private IEnumerator PlayDialogue(string targetText)
        {
            dialogueText.text = "";
            isTextTweening = true;
            isPlayerClick = false;

            // Chạy từng ký tự một
            for (int i = 0; i < targetText.Length; i++)
            {
                // Nếu người chơi click khi đang chạy chữ -> Show hết chữ ngay lập tức và dừng vòng lặp
                if (isPlayerClick)
                {
                    dialogueText.text = targetText;
                    isTextTweening = false;
                    isPlayerClick = false; // Reset click
                    break;
                }

                dialogueText.text += targetText[i];
                yield return new WaitForSeconds(textSpeed);
            }

            // Khi đã chạy xong toàn bộ text (hoặc đã bị ngắt để hiển thị full chữ)
            isTextTweening = false;

            // Vòng lặp chờ cú click tiếp theo để thực sự chuyển câu
            while (true)
            {
                yield return new WaitUntil(() => isPlayerClick);
                isPlayerClick = false; // Reset click
                break; // Thoát hàm để đi tới câu kế tiếp
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
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;

                PlayNpcTalkAnimation();
                yield return StartCoroutine(PlayDialogue(talkConfig.dialogue));
            }
            CompleteTutorial(TutorialState.Introduce);
        }
        private void PlayNpcTalkAnimation()
        {
            npcImage.transform.DOKill();

            npcImage.transform.localScale = Vector3.one;

            Sequence seq = DOTween.Sequence();

            seq.Append(
                npcImage.transform.DOScale(1.1f, 0.15f)
                    .SetEase(Ease.OutBack)
            );

            seq.Append(
                npcImage.transform.DOScale(1f, 0.1f)
                    .SetEase(Ease.InBack)
            );
        }
        #endregion

        #region State Use Skill
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
            if (currentState == TutorialState.UseSkill)
            {
                battleCtrlObj.SetActive(false);
                mainCamera.gameObject.SetActive(true);
                CompleteTutorial(TutorialState.UseSkill);

            }
            else if (currentState == TutorialState.CatchPokemon)
            {
                EndBattleCatch();
            }

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
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                PlayNpcTalkAnimation();
                yield return StartCoroutine(PlayDialogue(talkConfig.dialogue));
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
            PlayNpcTalkAnimation();
            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in completeChosePkmConfig.talkConfigs)
            {
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                PlayNpcTalkAnimation();
                yield return StartCoroutine(PlayDialogue(talkConfig.dialogue));
            }
            npcTalkPanel.SetActive(false);
            CompleteTutorial(TutorialState.ChosePokemon);

        }
        #endregion

        #region State Reward
        public void RewardTutorial()
        {
            rewardTutorialScreen.gameObject.SetActive(true);
            rewardTutorialScreen.OnClose += () =>
            {
                CompleteTutorial(TutorialState.Reward);
            };



        }
        #endregion
        #region State Catch Pokemon
        public IEnumerator IntroduceCatchPokemon()
        {

            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in catchPkmConfig.talkConfigs)
            {
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                PlayNpcTalkAnimation();
                yield return StartCoroutine(PlayDialogue(talkConfig.dialogue));
            }
            npcTalkPanel.SetActive(false);
            EnterBattleCatch();
        }
        public void EnterBattleCatch()
        {
            mainCamera.gameObject.SetActive(false);
            battleCamera.gameObject.SetActive(true);
            battleCtrlObj.SetActive(true);
            playerParty.Initialize();
            moreBtn.gameObject.SetActive(true);
            PokemonUnit enemyPkmUnit = new PokemonUnit(enemyPkm.pokemonData, enemyPkm.level);
            battleCtrl.StartBattleWithWildPokemon(playerParty, enemyPkmUnit, null, catchBg);
            handCache = Instantiate(hand, moreBtn.transform);
            handCache.anchoredPosition = new Vector2(0, 100);
            targetBtn = moreBtn;
            moreBtn.onClick.AddListener(SetupCatchBtn);
        }
        public void SetupCatchBtn()
        {
            moreBtn.onClick.RemoveListener(SetupCatchBtn);
            targetBtn = catchBtn;
            handCache.transform.SetParent(catchBtn.transform);
            handCache.anchoredPosition = new Vector2(0, 100);
            catchBtn.onClick.AddListener(DestroyHandCatch);
        }
        public void EndBattleCatch()
        {
            battleCtrlObj.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            StartCoroutine(CompleteCatchPokemon());
        }
        public IEnumerator CompleteCatchPokemon()
        {

            npcTalkPanel.SetActive(true);
            foreach (var talkConfig in completeCatchPkmConfig.talkConfigs)
            {
                npcImage.sprite = talkConfig.npcSprite;
                npcNameText.text = talkConfig.npcName;
                PlayNpcTalkAnimation();
                yield return StartCoroutine(PlayDialogue(talkConfig.dialogue));
            }
            npcTalkPanel.SetActive(false);
            AdvanceStep(TutorialState.CatchPokemon);
        }
        public void DestroyHandCatch()
        {
            if (handCache != null)
            {
                Destroy(handCache.gameObject);
                catchBtn.onClick.RemoveListener(DestroyHandCatch);
                targetBtn = null;
            }
        }
        #endregion
    }
}