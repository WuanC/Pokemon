using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PokemonDB
    {
        static Dictionary<string, PokemonData> pkmDictionary;
        public static IEnumerator Init()
        {
            pkmDictionary = new Dictionary<string, PokemonData>();
            var request = Resources.LoadAll<PokemonData>("Pokemons");
            yield return request;
            foreach (var pkm in request)
            {
                if (pkmDictionary.ContainsKey(pkm.pokemonName))
                {
                    Debug.LogWarning($"Duplicate pokemon name found: {pkm.pokemonName}. Skipping.");
                    continue;
                }
                pkmDictionary.Add(pkm.pokemonName, pkm);
            }
        }
        public static PokemonData GetPokemonByName(string pokemonName)
        {
            if (pkmDictionary.TryGetValue(pokemonName, out var pkmData))
            {
                return pkmData;
            }
            Debug.LogError($"Pokemon not found: {pokemonName}");
            return null;
        }
        public static List<PokemonData> GetAllPokemon()
        {
            return new List<PokemonData>(pkmDictionary.Values);
        }
    }
}