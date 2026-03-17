using System.Collections.Generic;
using UnityEngine;
using Pokemon.Scripts;
using Pokemon.Scripts.Pokemon;
using System.Collections;
using TMPro;
using DG.Tweening;

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
        private BattleState state = BattleState.Start;
        private int currentMoveIndex;
        private void Start()
        {

            playerPokemon.SetPokemon();
            playerPokemonUI.SetPokemon(playerPokemon.Pokemon, SetCurrentMove);

            enemyPokemon.SetPokemon();
            enemyPokemonUI.SetPokemon(enemyPokemon.Pokemon);
            skillText.gameObject.SetActive(false);
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
            yield return null;
            DamageDetails damageDetails = enemyPokemon.Pokemon.TakeDamage(selectedSkill, playerPokemon.Pokemon);
            float hpFraction = (float)enemyPokemon.Pokemon.HP / enemyPokemon.Pokemon.MaxHP;
            ShowSkillUI(damageDetails);
            enemyPokemonUI.UpdateHP(hpFraction, () =>
            {
                if (damageDetails.isFainted)
                {
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
            yield return new WaitForSeconds(1f);
            DamageDetails damageDetails = playerPokemon.Pokemon.TakeDamage(selectedSkill, enemyPokemon.Pokemon);
            float hpFraction = (float)playerPokemon.Pokemon.HP / playerPokemon.Pokemon.MaxHP;
            ShowSkillUI(damageDetails);
            playerPokemonUI.UpdateHP(hpFraction, () =>
            {
                if (damageDetails.isFainted)
                {
                    Debug.Log("Player Pokemon fainted!");
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

            Vector2 startPos = rect.anchoredPosition;

            rect.anchoredPosition = new Vector2(startPos.x - 200, startPos.y);

            skillText.gameObject.SetActive(true);

            rect.DOAnchorPosX(startPos.x, 0.5f)
                .OnComplete(() =>
                {
                    skillText.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        skillText.gameObject.SetActive(false);
                        skillText.rectTransform.anchoredPosition = startPos;
                        skillText.color = new Color(skillText.color.r, skillText.color.g, skillText.color.b, 1);
                    });
                });
        }

    }
}