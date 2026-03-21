
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Party
{
    public class PokemonParty : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI pokemonNameText;
        [SerializeField] TextMeshProUGUI pokemonLevelText;
        [SerializeField] Image pokemonIcon;
        [SerializeField] Image pokemonHpBar;
        public PokemonUnit Pokemon { get; private set; }
        public void SetPokemon(PokemonUnit pokemon)
        {
            Pokemon = pokemon;
            pokemonNameText.text = pokemon.Data.pokemonName;
            pokemonLevelText.text = "Lv." + pokemon.Level.ToString();
            pokemonIcon.sprite = pokemon.Data.icon;
            UpdateHpBar();
        }
        public void UpdateHpBar()
        {
            float hpPercent = (float)Pokemon.HP / Pokemon.MaxHP;
            pokemonHpBar.fillAmount = hpPercent;
        }
    }
}