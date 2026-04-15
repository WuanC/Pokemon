using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class PokemonLocker : MonoBehaviour
    {
        [SerializeField] private Image pokemonImage;
        [SerializeField] private Color lockerColor;
        public void Initialize(PokemonData pokemonData, bool isLocker)
        {
            pokemonImage.sprite = pokemonData.frontSprite;
            if (isLocker)
            {

                pokemonImage.color = lockerColor;
            }
            else
            {
                pokemonImage.color = Color.white;
            }
        }
    }
}