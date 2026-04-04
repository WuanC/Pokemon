
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonModal : MonoBehaviour
    {
        [SerializeField] private Image bg;
        [SerializeField] private TextMeshProUGUI pkmNameText;
        [SerializeField] private TextMeshProUGUI pkmLevelText;
        [SerializeField] private Image pkmImage;
        [SerializeField] private Image hpBar;
        public void SetModal(PokemonUnit pkmUnit)
        {
            pkmNameText.text = pkmUnit.Data.pokemonName;
            pkmLevelText.text = "Lv." + pkmUnit.Level;
            pkmImage.sprite = pkmUnit.Data.icon;
            hpBar.fillAmount = (float)pkmUnit.HP / pkmUnit.MaxHP;
        }
    }
}