
using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonModal : MonoBehaviour
    {
        [SerializeField] protected Image bg;
        [SerializeField] protected TextMeshProUGUI pkmNameText;
        [SerializeField] protected TextMeshProUGUI pkmLevelText;
        [SerializeField] protected Image pkmImage;
        [SerializeField] protected Image hpBar;
        [SerializeField] protected Image expBar;
        protected PokemonUnit pokemonUnit;
        public PokemonUnit PokemonUnit => pokemonUnit;
        protected bool hasExpBar;
        void OnEnable()
        {
            if (pokemonUnit == null) return;
            UpdateModal();
        }
        void OnDisable()
        {
            hpBar.DOKill();
            expBar.DOKill();
        }
        public void InitModal(PokemonUnit pkmUnit, bool hasExpBar = false)
        {
            pokemonUnit = pkmUnit;
            this.hasExpBar = hasExpBar;
            pkmNameText.text = pkmUnit.Data.pokemonName;
            pkmLevelText.text = "Lv." + pkmUnit.Level;
            if (pkmImage != null) pkmImage.sprite = pkmUnit.Data.icon;
            hpBar.fillAmount = (float)pkmUnit.HP / pkmUnit.MaxHP;
            if (hasExpBar)
            {
                expBar.transform.parent.gameObject.SetActive(true);
                SetupExp(pkmUnit);
            }
            else
            {
                expBar?.transform.parent.gameObject.SetActive(false);
            }
        }
        public void UpdateModal()
        {
            pkmLevelText.text = "Lv." + pokemonUnit.Level;
            hpBar.fillAmount = (float)pokemonUnit.HP / pokemonUnit.MaxHP;
            if (hasExpBar)
            {
                expBar.transform.parent.gameObject.SetActive(true);
                SetupExp(pokemonUnit);
            }
            else
            {
                expBar?.transform.parent.gameObject.SetActive(false);
            }
        }
        public void UpdateTextLevel()
        {
            pkmLevelText.text = "Lv. " + pokemonUnit.Level.ToString();
        }
        public IEnumerator UpdateHP(float hpFraction, float duration)
        {
            yield return hpBar.DOFillAmount(hpFraction, duration).WaitForCompletion();
        }
        public void SetupExp(PokemonUnit pokemon)
        {
            int expNextLevelNormalized = pokemon.CalculateExpYield(pokemon.Level + 1) - pokemon.CalculateExpYield(pokemon.Level);
            int currentExpNormalized = pokemon.CurrentExp - pokemon.CalculateExpYield(pokemon.Level);
            expBar.fillAmount = (float)currentExpNormalized / expNextLevelNormalized;
        }
        public IEnumerator UpdateExpBar(bool isReset = false)
        {
            if (isReset)
            {
                UpdateTextLevel();
                expBar.fillAmount = 0;
            }
            int expNextLevelNormalized = pokemonUnit.CalculateExpYield(pokemonUnit.Level + 1) - pokemonUnit.CalculateExpYield(pokemonUnit.Level);
            int currentExpNormalized = pokemonUnit.CurrentExp - pokemonUnit.CalculateExpYield(pokemonUnit.Level);
            yield return expBar.DOFillAmount((float)currentExpNormalized / expNextLevelNormalized, 0.5f).WaitForCompletion();
        }
    }
}