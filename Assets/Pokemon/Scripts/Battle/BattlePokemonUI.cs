
using System;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class BattlePokemonUI : MonoBehaviour
    {
        private PokemonUnit pokemon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image hpBar;
        [Header("Skills")]
        public Transform skillsParent;
        public Button skillBtnPrefab;
        public void SetPokemon(PokemonUnit pokemon, Action<int> onSkillSelected = null)
        {
            this.pokemon = pokemon;
            nameText.text = pokemon.Data.name;
            levelText.text = "Lv. " + pokemon.Level.ToString();
            hpBar.fillAmount = 1f;
            SetSkillButtons(onSkillSelected);

        }
        public void SetSkillButtons(Action<int> onSkillSelected = null)
        {
            if (skillsParent == null) return;
            foreach (Transform child in skillsParent)
            {
                Destroy(child.gameObject);
            }
            foreach (var skill in pokemon.Skills)
            {
                Button btn = Instantiate(skillBtnPrefab, skillsParent);
                btn.GetComponent<Image>().sprite = skill.Data.icon;
                btn.onClick.AddListener(() => onSkillSelected?.Invoke(pokemon.Skills.IndexOf(skill)));
            }
        }
        public void UpdateHP(float hpFraction, Action onComplete = null)
        {
            hpBar.DOFillAmount(hpFraction, 0.5f).OnComplete(() => onComplete?.Invoke());
        }
    }
}