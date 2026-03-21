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

namespace Pokemon.Scripts.Battle
{
    public enum BattleState
    {
        Start,
        PlayerAction,
        PlayerMove,
        EnemyMove,
    }
    public class BattleController : MonoBehaviour
    {
        [SerializeField] BattlePokemon playerPokemon;
        [SerializeField] BattlePokemonUI playerPokemonUI;
        [SerializeField] BattlePokemon enemyPokemon;
        [SerializeField] BattlePokemonUI enemyPokemonUI;
        [SerializeField] TextMeshProUGUI skillText;
        [SerializeField] Image typeEffectImage;
        [SerializeField] Image criticalImage;
        private BattleState state = BattleState.Start;
        private int currentMoveIndex;
        [SerializeField] private Sprite strongSprite;
        [SerializeField] private Sprite weakSprite;
        [SerializeField] private Sprite extraSprite;
        private Pokemon.Party playerParty;
        [SerializeField] private PartyContainer partyContainer;
        [SerializeField] private MainPanel mainPanel;
        [SerializeField] private MorePanel morePanel;
        void Start()
        {
            Observer.Instance.Register(EventId.OnSwitchPokemon, OnSwitchPokemon);
        }
        void Oestroy()
        {
            Observer.Instance.Unregister(EventId.OnSwitchPokemon, OnSwitchPokemon);
        }
        public void StartBattle(Pokemon.Party party, PokemonUnit wildPokemon)
        {

            this.playerParty = party;
            PokemonUnit playerPkmUnit = party.GetHealthyPokemon();
            if (playerPkmUnit == null)
            {
                BroadCast(false);
                Debug.LogError("No healthy Pokemon in the party!");
                return;
            }
            partyContainer.SetParty(party.PokemonParties.Where(p => p != playerPkmUnit).ToList());
            playerPokemon.SetPokemon(playerPkmUnit);
            playerPokemonUI.SetPokemon(playerPkmUnit, SetCurrentMove);

            enemyPokemon.SetPokemon(wildPokemon);
            enemyPokemonUI.SetPokemon(wildPokemon);
            skillText.gameObject.SetActive(false);
            typeEffectImage.gameObject.SetActive(false);
            criticalImage.gameObject.SetActive(false);
            PlayerAction();
        }
        public void PlayerAction()
        {
            state = BattleState.PlayerAction;
        }
        public void SetCurrentMove(int moveIndex)
        {
            if (state != BattleState.PlayerAction) return;
            currentMoveIndex = moveIndex;
            StartCoroutine(PlayerMove());

        }
        public IEnumerator PlayerMove()
        {
            state = BattleState.PlayerMove;
            Skill selectedSkill = playerPokemon.Pokemon.Skills[currentMoveIndex];

            DamageDetails damageDetails = enemyPokemon.Pokemon.TakeDamage(selectedSkill, playerPokemon.Pokemon);
            playerPokemon.AttackAnimation();
            ShowSkillUI(damageDetails);

            yield return new WaitForSeconds(1f);
            enemyPokemon.HitAnimation();
            float hpFraction = (float)enemyPokemon.Pokemon.HP / enemyPokemon.Pokemon.MaxHP;
            enemyPokemonUI.UpdateHP(hpFraction, () =>
            {
                if (damageDetails.isFainted)
                {
                    enemyPokemon.FaintAnimation(() => BroadCast(true));
                    Debug.Log("Enemy Pokemon fainted!");
                }
                else
                {
                    StartCoroutine(EnemyMove());
                }
            });

        }
        public IEnumerator EnemyMove()
        {
            state = BattleState.EnemyMove;
            Skill selectedSkill = enemyPokemon.Pokemon.RandomSkill();
            enemyPokemon.AttackAnimation();
            DamageDetails damageDetails = playerPokemon.Pokemon.TakeDamage(selectedSkill, enemyPokemon.Pokemon);
            ShowSkillUI(damageDetails);
            yield return new WaitForSeconds(1f);
            playerPokemon.HitAnimation();
            float hpFraction = (float)playerPokemon.Pokemon.HP / playerPokemon.Pokemon.MaxHP;

            playerPokemonUI.UpdateHP(hpFraction, () =>
            {
                if (damageDetails.isFainted)
                {
                    PokemonUnit nextPokemon = playerParty.GetHealthyPokemon();
                    if (nextPokemon != null)
                    {
                        playerPokemon.FaintAnimation(() =>
                        {
                            playerPokemon.SetPokemon(nextPokemon);
                            playerPokemonUI.SetPokemon(nextPokemon, SetCurrentMove);
                            PlayerAction();
                        });
                        return;
                    }
                    else
                    {
                        playerPokemon.FaintAnimation(() => BroadCast(false));
                        Debug.Log("Player Pokemon fainted!");
                    }

                }
                else
                {
                    PlayerAction();
                }
            });

        }
        public void ShowSkillUI(DamageDetails damageDetails)
        {
            skillText.text = damageDetails.skillName;

            RectTransform rect = skillText.rectTransform;
            Vector2 nameStartPos = rect.anchoredPosition;

            rect.anchoredPosition = new Vector2(nameStartPos.x - 200, nameStartPos.y);
            skillText.gameObject.SetActive(true);
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
            rect.DOAnchorPosX(nameStartPos.x, 0.5f)
                .OnComplete(() =>
                {
                    skillText.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        skillText.gameObject.SetActive(false);
                        skillText.rectTransform.anchoredPosition = nameStartPos;
                        skillText.color = new Color(skillText.color.r, skillText.color.g, skillText.color.b, 1);
                    });
                });
        }
        public void BroadCast(bool isWin)
        {
            Observer.Instance.Broadcast(EventId.OnEndBattle, isWin);
        }
        #region Switch Pokemon
        public void OnSwitchPokemon(object data)
        {
            if (state != BattleState.PlayerAction) return;
            if (data is Party.PokemonParty pokemonParty)
            {
                PokemonUnit playerPkmUnit = playerPokemon.Pokemon;
                PokemonUnit partyPokemon = pokemonParty.Pokemon;
                float duration = 0.25f;
                morePanel.DisablePanel(duration);
                playerPokemon.ExitAnimation(() =>
                {
                    playerPokemon.SetPokemon(partyPokemon, duration);
                    playerPokemonUI.SetPokemon(partyPokemon, SetCurrentMove);
                }, duration);
                pokemonParty.SetPokemon(playerPkmUnit);
            }
        }
        #endregion

    }
}