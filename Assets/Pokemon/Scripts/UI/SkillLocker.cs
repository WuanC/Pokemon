
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class SkillLocker : MonoBehaviour
    {
        [SerializeField] Image lockSkill;
        [SerializeField] Image lockBg;
        [SerializeField] TextMeshProUGUI lvUnlockText;
        private PokemonUnit pokemonUnit;
        private PokemonSkill pokemonSkill;
        public void Initialize(PokemonUnit pokemonUnit, PokemonSkill pokemonSkill)
        {
            this.pokemonUnit = pokemonUnit;
            this.pokemonSkill = pokemonSkill;
            lockSkill.sprite = pokemonSkill.skillData.icon;
            SetLock();
        }
        public void SetLock()
        {
            if (pokemonUnit.Level < pokemonSkill.levelRequirement)
            {
                lockSkill.color = new Color(1, 1, 1, 0.2f);
                lockBg.gameObject.SetActive(true);
                lvUnlockText.text = "Lv. " + pokemonSkill.levelRequirement;
            }
            else
            {
                lockSkill.color = Color.white;
                lockBg.gameObject.SetActive(false);
            }
        }
    }
}