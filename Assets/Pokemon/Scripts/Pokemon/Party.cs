using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    [System.Serializable]
    public class PokemonParty
    {
        public PokemonData pokemonData;
        public int level;
    }
    public class Party : MonoBehaviour
    {
        [SerializeField] PokemonParty[] pokemonParties;
        public List<PokemonUnit> PokemonParties { get; private set; }

        public Party(List<PokemonUnit> pokemonParties)
        {
            this.PokemonParties = pokemonParties;
        }
        void Start()
        {
            InitParty();
        }
        public void InitParty()
        {
            PokemonParties = new List<PokemonUnit>();
            for (int i = 0; i < pokemonParties.Length; i++)
            {
                PokemonUnit pokemonUnit = new PokemonUnit(pokemonParties[i].pokemonData, pokemonParties[i].level);
                PokemonParties.Add(pokemonUnit);
            }
        }
        public PokemonUnit GetHealthyPokemon()
        {
            return PokemonParties.FirstOrDefault(p => p.HP > 0);
        }
    }
}