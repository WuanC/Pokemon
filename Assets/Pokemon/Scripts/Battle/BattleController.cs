using System.Collections.Generic;
using UnityEngine;
using Pokemon.Scripts;
using Pokemon.Scripts.Pokemon;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Party;
using System.Linq;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Pokemon.Scripts.Character;

namespace Pokemon.Scripts.Battle
{
    public enum BattleState
    {
        Start,
        PlayerAction,
        Busy,
        Running,
        Over,
    }
    public enum BattleAction
    {
        None,
        Fight,
        Switch,
        UseItem,
        Run,
    }
    public class BattleController : MonoBehaviour
    {
        [SerializeField] BattlePokemon playerPokemon;
        [SerializeField] BattlePokemon enemyPokemon;
        [SerializeField] TextMeshProUGUI skillText;
        [SerializeField] Image typeEffectImage;
        [SerializeField] Image criticalImage;
        private BattleState state = BattleState.Start;
        private int currentMoveIndex;
        [SerializeField] private Sprite strongSprite;
        [SerializeField] private Sprite weakSprite;
        [SerializeField] private Sprite extraSprite;

        [SerializeField] private PartyContainer partyContainer;
        [SerializeField] private MainPanel mainPanel;
        [SerializeField] private MorePanel morePanel;
        [SerializeField] private BattleAction battleAction;
        private Pokemon.Party playerParty;
        private Pokemon.Party enemyParty;
        private bool isNPCBattle;
        [Header("Trainer")]
        [SerializeField] private Image npcImage;
        [SerializeField] private TextMeshProUGUI npcNameText;
        void Start()
        {
            Observer.Instance.Register(EventId.OnSwitchPokemon, OnPlayerSwitchPokemon);
        }
        private void OnDestroy()
        {
            Observer.Instance.Unregister(EventId.OnSwitchPokemon, OnPlayerSwitchPokemon);
            skillText.DOKill();
            typeEffectImage.DOKill();
            criticalImage.DOKill();
        }
        #region Setup Battle
        public void StartBattleWithWildPokemon(Pokemon.Party party, PokemonUnit enemyPokemon)
        {
            npcImage.gameObject.SetActive(false);
            npcNameText.gameObject.SetActive(false);
            isNPCBattle = false;
            this.playerParty = party;
            PokemonUnit playerPkmUnit = party.GetHealthyPokemon();
            partyContainer.SetParty(party.PokemonParties.Where(p => p != playerPkmUnit).ToList());
            playerPokemon.SetPokemon(playerPkmUnit, SetCurrentMove);
            this.enemyPokemon.SetPokemon(enemyPokemon);
            DisableTextAndEffects();
            SetPlayerAction();
            battleAction = BattleAction.None;
        }
        public void StartBattleWithNPC(Pokemon.Party party, NPC npc)
        {
            npcImage.gameObject.SetActive(true);
            npcNameText.gameObject.SetActive(true);
            npcImage.sprite = npc.npcData.npcSprite;
            npcNameText.text = npc.npcData.npcName;
            isNPCBattle = true;
            this.playerParty = party;
            this.enemyParty = npc.party;
            PokemonUnit playerPkmUnit = party.GetHealthyPokemon();
            partyContainer.SetParty(party.PokemonParties.Where(p => p != playerPkmUnit).ToList());
            playerPokemon.SetPokemon(playerPkmUnit, SetCurrentMove);
            enemyPokemon.SetPokemon(enemyParty.GetHealthyPokemon());
            DisableTextAndEffects();
            SetPlayerAction();
        }
        public void DisableTextAndEffects()
        {
            skillText.gameObject.SetActive(false);
            typeEffectImage.gameObject.SetActive(false);
            criticalImage.gameObject.SetActive(false);
        }
        #endregion
        #region Command
        public void SetPlayerAction()
        {
            state = BattleState.PlayerAction;
        }
        public void SetCurrentMove(int moveIndex)
        {
            if (state != BattleState.PlayerAction) return;
            currentMoveIndex = moveIndex;
            battleAction = BattleAction.Fight;
            StartCoroutine(RunTurn());
        }
        public void RunBattle()
        {
            if (state != BattleState.PlayerAction) return;
            battleAction = BattleAction.Run;
            StartCoroutine(RunTurn());
        }
        public IEnumerator PlayerSwitchPokemon(PartySlot partySlot)
        {
            PokemonUnit playerPkmUnit = playerPokemon.Pokemon;
            PokemonUnit partyPokemon = partySlot.Pokemon;
            OpenMainPanel();
            playerPokemon.ExitAnimation(0.25f);
            partySlot.SetPokemon(playerPkmUnit);
            yield return new WaitForSeconds(0.25f);
            playerPokemon.SetPokemon(partyPokemon, SetCurrentMove, 0.25f);
            yield return new WaitForSeconds(0.25f);
            state = BattleState.Running;
            if (battleAction == BattleAction.None)
            {
                battleAction = BattleAction.Switch;
                StartCoroutine(RunTurn());
            }
        }
        public IEnumerator NPCSwitchPokemon(PokemonUnit nextPokemon)
        {
            state = BattleState.Busy;
            PokemonUnit enemyPkmUnit = enemyPokemon.Pokemon;
            PokemonUnit partyPokemon = nextPokemon;
            enemyPokemon.ExitAnimation(0.25f);
            yield return new WaitForSeconds(0.25f);
            enemyPokemon.SetPokemon(partyPokemon, null, 0.25f);
            yield return new WaitForSeconds(0.25f);
            state = BattleState.Running;
        }
        public void OnPlayerSwitchPokemon(object data)
        {
            if (state != BattleState.PlayerAction) return;
            if (data is PartySlot partySlot)
            {
                StartCoroutine(PlayerSwitchPokemon(partySlot));
            }
        }
        #endregion
        public IEnumerator RunTurn()
        {
            state = BattleState.Running;
            if (battleAction == BattleAction.Fight)
            {
                playerPokemon.CurrentSkill = playerPokemon.Pokemon.Skills[currentMoveIndex];
                enemyPokemon.CurrentSkill = enemyPokemon.Pokemon.RandomSkill();
                bool playerGoesFirst = true;
                if (playerPokemon.CurrentSkill.Data.skillPriority < enemyPokemon.CurrentSkill.Data.skillPriority)
                {
                    playerGoesFirst = false;
                }
                else if (playerPokemon.CurrentSkill.Data.skillPriority == enemyPokemon.CurrentSkill.Data.skillPriority)
                {
                    playerGoesFirst = playerPokemon.Pokemon.Speed >= enemyPokemon.Pokemon.Speed;
                }

                var firstUnit = playerGoesFirst ? playerPokemon : enemyPokemon;
                var secondUnit = playerGoesFirst ? enemyPokemon : playerPokemon;

                yield return ActionMove(firstUnit, secondUnit, firstUnit.CurrentSkill);
                yield return CheckPokemonFainted(secondUnit);
                if (state == BattleState.Over) yield break;
                if (secondUnit.Pokemon.HP > 0)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return ActionMove(secondUnit, firstUnit, secondUnit.CurrentSkill);
                    yield return CheckPokemonFainted(firstUnit);
                    if (state == BattleState.Over) yield break;
                }

                yield return new WaitUntil(() => state == BattleState.Running);
            }
            else if (battleAction == BattleAction.Switch)
            {
                enemyPokemon.CurrentSkill = enemyPokemon.Pokemon.RandomSkill();
                yield return ActionMove(enemyPokemon, playerPokemon, enemyPokemon.CurrentSkill);
                yield return CheckPokemonFainted(playerPokemon);
                if (state == BattleState.Over) yield break;
                yield return new WaitUntil(() => state == BattleState.Running);
            }
            else if (battleAction == BattleAction.Run)
            {
                OpenMainPanel();
                BroadCast(false);
                yield break;
            }
            battleAction = BattleAction.None;
            SetPlayerAction();
            yield return null;
        }
        public IEnumerator ActionMove(BattlePokemon attacker, BattlePokemon defender, Skill skill)
        {
            StartCoroutine(ShowSkillName(skill.Data.name));
            yield return new WaitForSeconds(0.5f);
            yield return attacker.AttackAnimation().WaitForCompletion();
            defender.HitAnimation();
            GameObject skillFx = null;
            if (skill.Data.vfxPrefab != null)
            {
                skillFx = MyPoolManager.Instance.GetFromPool(skill.Data.vfxPrefab);
                skillFx.transform.position = defender.pokemonImage.transform.position;
            }
            if (skill.Data.category == CategorySkill.Status)
            {
                foreach (var statBoost in skill.Data.statBoosts)
                {
                    if (skill.Data.targetType == TargetType.Self)
                    {
                        float lastValue = attacker.Pokemon.GetStat(statBoost.stat);
                        attacker.Pokemon.ApplyBoost(statBoost);
                        float newValue = attacker.Pokemon.GetStat(statBoost.stat);
                        attacker.UpdateStatUI(statBoost, lastValue, newValue, 1f);
                    }
                    else
                    {
                        float lastValue = defender.Pokemon.GetStat(statBoost.stat);
                        defender.Pokemon.ApplyBoost(statBoost);
                        float newValue = defender.Pokemon.GetStat(statBoost.stat);
                        defender.UpdateStatUI(statBoost, lastValue, newValue, 1f);
                    }
                }
                yield return new WaitForSeconds(1f);
            }
            else if (skill.Data.category == CategorySkill.Attack)
            {
                DamageDetails damageDetails = defender.Pokemon.TakeDamage(skill, attacker.Pokemon);
                ShowSkillUI(damageDetails);
                float hpFraction = (float)defender.Pokemon.HP / defender.Pokemon.MaxHP;
                yield return defender.UpdateHp(hpFraction, 0.5f).WaitForCompletion();

            }
            if (skillFx != null)
            {
                skillFx.gameObject.SetActive(false);
            }
        }
        public IEnumerator CheckPokemonFainted(BattlePokemon defender)
        {
            if (defender.Pokemon.HP <= 0)
            {
                defender.ExitAnimation(0.25f);
                yield return new WaitForSeconds(0.25f);
                CheckBattleOver(defender);
            }
        }
        public void CheckBattleOver(BattlePokemon defender)
        {
            if (defender.IsPlayerPokemon)
            {
                PokemonUnit playerPkmUnit = playerParty.GetHealthyPokemon();
                if (playerPkmUnit == null)
                {
                    state = BattleState.Over;
                    BroadCast(false);
                }
                else
                {
                    state = BattleState.Busy;
                    SetPlayerAction();
                    OpenMorePanel(true);
                }
            }
            else
            {
                if (!isNPCBattle)
                {
                    state = BattleState.Over;
                    BroadCast(true);
                    return;
                }
                else
                {
                    PokemonUnit playerPkmUnit = enemyParty.GetHealthyPokemon();
                    if (playerPkmUnit == null)
                    {
                        state = BattleState.Over;
                        BroadCast(true);
                        return;
                    }
                    else
                    {
                        state = BattleState.Busy;
                        StartCoroutine(NPCSwitchPokemon(playerPkmUnit));
                    }
                }
            }
        }
        #region Show UI
        public IEnumerator ShowSkillName(string skillName)
        {
            skillText.text = skillName;
            RectTransform rect = skillText.rectTransform;
            Vector2 nameStartPos = rect.anchoredPosition;
            rect.anchoredPosition = new Vector2(nameStartPos.x - 200, nameStartPos.y);
            skillText.gameObject.SetActive(true);
            rect.DOAnchorPosX(nameStartPos.x, 0.5f);
            yield return new WaitForSeconds(0.8f);
            skillText.DOFade(0, 0.5f).OnComplete(() =>
            {
                skillText.gameObject.SetActive(false);
                skillText.color = new Color(skillText.color.r, skillText.color.g, skillText.color.b, 1);
                rect.anchoredPosition = nameStartPos;
            });
        }
        public void ShowSkillUI(DamageDetails damageDetails)
        {
            if (damageDetails.critical >= 2)
            {
                criticalImage.transform.localScale = Vector3.zero;
                criticalImage.gameObject.SetActive(true);
                criticalImage.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    criticalImage.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        criticalImage.gameObject.SetActive(false);
                        criticalImage.color = new Color(criticalImage.color.r, criticalImage.color.g, criticalImage.color.b, 1);
                    });
                });
            }
            if (damageDetails.typeEffectiveness != 1)
            {
                typeEffectImage.sprite = damageDetails.typeEffectiveness == 2 ? strongSprite : damageDetails.typeEffectiveness == 0.5f ? weakSprite : extraSprite;
                typeEffectImage.transform.localScale = Vector3.zero;
                typeEffectImage.gameObject.SetActive(true);
                typeEffectImage.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {

                    typeEffectImage.gameObject.SetActive(false);
                    typeEffectImage.color = new Color(typeEffectImage.color.r, typeEffectImage.color.g, typeEffectImage.color.b, 1);
                });
            }

        }
        #endregion
        public void BroadCast(bool isWin)
        {
            Observer.Instance.Broadcast(EventId.OnEndBattle, isWin);
        }

        public void OpenMainPanel()
        {
            morePanel.DisablePanel(0.25f, () =>
            {
                mainPanel.EnablePanel(0.25f);
            });
        }
        public void OpenMorePanel(bool forceSelect = false)
        {
            mainPanel.DisablePanel(0.25f, () =>
            {
                morePanel.EnablePanel(0.25f, forceSelect);
            });
        }

    }
}