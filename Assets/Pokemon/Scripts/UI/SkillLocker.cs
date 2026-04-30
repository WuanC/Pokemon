
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class SkillLocker : MonoBehaviour
    {
        [SerializeField] private Button skillButton;
        [SerializeField] Image lockSkill;
        [SerializeField] Image lockBg;
        [SerializeField] TextMeshProUGUI lvUnlockText;
        private PokemonUnit pokemonUnit;
        private PokemonSkill pokemonSkill;
        private SkillDetailScreen skillDetailScreen;
        public void Initialize(PokemonUnit pokemonUnit, PokemonSkill pokemonSkill, SkillDetailScreen skillDetailScreen)
        {
            this.pokemonUnit = pokemonUnit;
            this.pokemonSkill = pokemonSkill;
            this.skillDetailScreen = skillDetailScreen;
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
                skillButton.onClick.AddListener(() =>
{
    skillDetailScreen.Initialize(pokemonUnit, pokemonSkill.skillData, false);
});
            }
            else
            {
                if (pokemonUnit.ContainsSkill(pokemonSkill.skillData))
                {
                    lockSkill.color = Color.white;
                    skillButton.onClick.AddListener(() =>
{
    skillDetailScreen.Initialize(pokemonUnit, pokemonSkill.skillData, false);
});
                }
                else
                {
                    lockSkill.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                    skillButton.onClick.AddListener(() =>
                    {
                        skillDetailScreen.Initialize(pokemonUnit, pokemonSkill.skillData, true);
                    });
                }
                lockBg.gameObject.SetActive(false);
            }
        }
        void OnDisable()
        {
            skillButton.onClick.RemoveAllListeners();
        }
    }
}