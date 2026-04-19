using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Scripts.Saving;
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

        [SerializeField] protected PokemonParty[] initPokemonParties;
        public List<PokemonUnit> PokemonParties { get; protected set; }

        public virtual void Initialize()
        {
            PokemonParties = new List<PokemonUnit>();
            InitParty();
        }
        public virtual void InitParty()
        {

            for (int i = 0; i < initPokemonParties.Length; i++)
            {
                PokemonUnit pokemonUnit = new PokemonUnit(initPokemonParties[i].pokemonData, initPokemonParties[i].level);
                PokemonParties.Add(pokemonUnit);
            }
        }
        public virtual PokemonUnit GetHealthyPokemon()
        {
            return PokemonParties.FirstOrDefault(p => p.HP > 0);
        }
    }
}