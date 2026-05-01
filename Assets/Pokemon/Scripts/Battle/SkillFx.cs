using System.Collections;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Unity.VisualScripting;
using UnityEngine;

namespace Pokemon.Scripts.Battle
{
    [CreateAssetMenu(fileName = "New Skill Fx", menuName = "Pokemon/Skills/Skill Fx")]
    public class SkillFx : ScriptableObject
    {
        [SerializeField] protected GameObject targetFxPrefab;
        [SerializeField] protected GameObject ownerFxPrefab;
        [SerializeField] protected float fxDuration;
        public IEnumerator CastEffectCoroutine(BattlePokemon owner, BattlePokemon target)
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
    }
}