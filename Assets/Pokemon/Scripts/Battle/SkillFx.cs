using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using UnityEngine;

namespace Pokemon.Scripts.Battle
{
    public enum SkillFxType
    {
        Default,
        ManyProjectiles,
    }
    [CreateAssetMenu(fileName = "New Skill Fx", menuName = "Pokemon/Skills/Skill Fx")]
    public class SkillFx : ScriptableObject
    {
        [SerializeField] protected GameObject targetFxPrefab;
        [SerializeField] protected GameObject ownerFxPrefab;
        [SerializeField] protected float fxDuration;
        [SerializeField] protected SkillFxType fxType;
        public IEnumerator CastEffectCoroutine(BattlePokemon owner, BattlePokemon target)
        {

            if (fxType == SkillFxType.Default)
            {
                GameObject ownerFx = null;
                GameObject targetFx = null;
                if (ownerFxPrefab != null)
                {
                    ownerFx = MyPoolManager.Instance.GetFromPool(ownerFxPrefab);
                    ownerFx.transform.position = owner.pokemonImage.transform.position;

                }
                if (targetFxPrefab != null)
                {
                    targetFx = MyPoolManager.Instance.GetFromPool(targetFxPrefab);
                    targetFx.transform.position = target.pokemonImage.transform.position;
                    target.HitAnimation();
                }
                yield return new WaitForSeconds(fxDuration);
                if (ownerFx != null) ownerFx.SetActive(false);
                if (targetFx != null) targetFx.SetActive(false);

            }
            else if (fxType == SkillFxType.ManyProjectiles)
            {
                if (ownerFxPrefab != null)
                {
                    Sequence sequence = DOTween.Sequence();

                    for (int i = 0; i < 5; i++)
                    {
                        float spawnDelay = i * 0.1f;

                        sequence.Insert(spawnDelay, DOTween.Sequence().AppendCallback(() =>
                        {
                            GameObject projectile = MyPoolManager.Instance.GetFromPool(ownerFxPrefab);

                            projectile.transform.position = owner.pokemonImage.transform.position;

                            projectile.transform.DOMove(
                                target.pokemonImage.transform.position,
                                fxDuration
                            ).OnComplete(() =>
                            {
                                projectile.SetActive(false);
                                target.HitAnimation();
                            });
                        }));
                    }

                    yield return sequence.WaitForCompletion();
                }
            }
        }
    }
}