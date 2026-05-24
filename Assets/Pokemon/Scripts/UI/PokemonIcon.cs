using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonIcon : MonoBehaviour
    {
        private Image iconImage;
        private string pokemonName;

        public string PokemonName => pokemonName;
        public void SetIcon(Sprite icon, string pokemonName)
        {
            if (iconImage == null)
                iconImage = GetComponent<Image>();
            iconImage.sprite = icon;
            this.pokemonName = pokemonName;
        }
        public void SetStatus(bool isCaught)
        {
            if (iconImage == null)
                iconImage = GetComponent<Image>();
            iconImage.color = isCaught ? Color.white : Color.gray;
        }
    }
}