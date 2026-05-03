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
        [SerializeField] protected GameObject mapFxPrefab;
        [SerializeField] protected float fxDuration;

        public virtual IEnumerator CastEffectCoroutine(BattlePokemon owner, BattlePokemon target)
        {

            GameObject ownerFx = null;
            GameObject targetFx = null;
            GameObject mapFx = null;
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
            if (mapFxPrefab != null)
            {
                mapFx = MyPoolManager.Instance.GetFromPool(mapFxPrefab);
                mapFx.transform.position = (target.pokemonImage.transform.position + owner.pokemonImage.transform.position) / 2;
            }
            yield return new WaitForSeconds(fxDuration);
            if (ownerFx != null) ownerFx.SetActive(false);
            if (targetFx != null) targetFx.SetActive(false);
            if (mapFx != null) mapFx.SetActive(false);

        }
    }
}