using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using UnityEngine;

namespace Pokemon.Scripts.Battle
{
    [CreateAssetMenu(fileName = "New Skill Fx", menuName = "Pokemon/Skills/Projectile Fx")]
    public class ProjectileSkillFx : SkillFx
    {
        [SerializeField] protected int projectilesCount;
        [SerializeField] protected float projectileSpawnInterval;
        [SerializeField] protected float projectileLoadDuration;
        [SerializeField] protected float moveDuration;
        [SerializeField] protected float targetFxDuration;
        public override IEnumerator CastEffectCoroutine(BattlePokemon owner, BattlePokemon target)
        {

            if (ownerFxPrefab != null)
            {



                for (int i = 0; i < projectilesCount; i++)
                {

                    GameObject projectile = MyPoolManager.Instance.GetFromPool(ownerFxPrefab);
                    projectile.transform.position = owner.pokemonImage.transform.position;
                    yield return new WaitForSeconds(projectileLoadDuration);
                    Vector2 dir = (target.pokemonImage.transform.position - owner.pokemonImage.transform.position).normalized;

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    //projectile.transform.DORotate(new Vector3(0, 0, angle), 0.1f);
                    projectile.transform.localScale = target.IsPlayerPokemon ? new Vector3(-1, -1, 1) : Vector3.one;
                    projectile.transform.DOMove(
                                target.pokemonImage.transform.position,
                                moveDuration
                            ).OnComplete(() =>
                            {
                                projectile.SetActive(false);
                                target.HitAnimation();
                                target.StartCoroutine(SetTargetFxCoroutine(target));

                            });
                    yield return new WaitForSeconds(projectileSpawnInterval);
                }
                yield return new WaitForSeconds(moveDuration + targetFxDuration);



            }
        }
        public IEnumerator SetTargetFxCoroutine(BattlePokemon target)
        {
            if (targetFxPrefab != null)
            {
                GameObject targetFx = MyPoolManager.Instance.GetFromPool(targetFxPrefab);
                targetFx.transform.position = target.pokemonImage.transform.position;
                yield return new WaitForSeconds(targetFxDuration);
                targetFx.SetActive(false);
            }
        }

    }
}