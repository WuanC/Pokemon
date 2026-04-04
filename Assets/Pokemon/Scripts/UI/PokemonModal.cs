
using Pokemon.Scripts.Pokemon;
using TMPro;
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

        protected PokemonUnit pokemonUnit;
        public void SetModal(PokemonUnit pkmUnit)
        {
            pokemonUnit = pkmUnit;
            pkmNameText.text = pkmUnit.Data.pokemonName;
            pkmLevelText.text = "Lv." + pkmUnit.Level;
            pkmImage.sprite = pkmUnit.Data.icon;
            hpBar.fillAmount = (float)pkmUnit.HP / pkmUnit.MaxHP;
        }
    }
}