
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class BattlePokemon : MonoBehaviour
    {
        [SerializeField] PokemonData data;
        [SerializeField] int level;
        [SerializeField] bool isPlayerPokemon;
        [SerializeField] Image pokemonImage;
        public PokemonUnit Pokemon { get; private set; }
        public void SetPokemon()
        {
            Pokemon = new PokemonUnit(data, level);
            if (isPlayerPokemon)
            {
                pokemonImage.sprite = data.backSprite;
            }
            else
            {
                pokemonImage.sprite = data.frontSprite;
            }
        }
    }
}