using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Saving;
using Unity.VisualScripting;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PlayerParty : Party, ISavable
    {
        public static PlayerParty Instance { get; private set; }
        private const string dexKey = "PokemonDex";
        private const string partyKey = "PokemonParty";
        private const string inventoryKey = "PokemonInventory";

        public List<PokemonUnit> inventory { get; private set; }
        public HashSet<string> pokedex { get; private set; } = new();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public override void Initialize()
        {
            PokemonParties = new List<PokemonUnit>();
            if (RestoreState() != null)
            {
                List<PokemonSaveData> saveData = RestoreState() as List<PokemonSaveData>;
                foreach (var pokemonData in saveData)
                {
                    PokemonUnit pokemonUnit = new PokemonUnit(pokemonData);
                    PokemonParties.Add(pokemonUnit);
                }
            }
            else
            {
                InitParty();
                foreach (var pokemon in PokemonParties)
                {
                    Debug.Log("Add " + pokemon.Data.pokemonName + " to pokedex");
                    pokedex.Add(pokemon.Data.pokemonName);
                    PostEventAddPkmon(pokemon.Data.pokemonName);
                }
            }


        }

        private void OnDestroy()
        {
            CaptureState();
        }
        public void AddPokemon(PokemonUnit pokemon)
        {
            if (PokemonParties.Count < 4)
            {
                PokemonParties.Add(pokemon);

            }
            else
            {
                if (inventory == null)
                {
                    inventory = new List<PokemonUnit>();
                }
                inventory.Add(pokemon);
            }
            pokedex.Add(pokemon.Data.pokemonName);
            PostEventAddPkmon(pokemon.Data.pokemonName);
        }

        public void CaptureState()
        {
            List<PokemonSaveData> partySaveData = PokemonParties.Select(p => p.GetSaveData()).ToList();
            string partyJson = JsonConvert.SerializeObject(partySaveData, Formatting.Indented);
            PlayerPrefs.SetString(partyKey, partyJson);

            string dexJson = JsonConvert.SerializeObject(pokedex.ToList(), Formatting.Indented);
            PlayerPrefs.SetString(dexKey, dexJson);

            if (inventory != null)
            {
                List<PokemonSaveData> inventorySaveData = inventory.Select(p => p.GetSaveData()).ToList();
                string inventoryJson = JsonConvert.SerializeObject(inventorySaveData, Formatting.Indented);
                PlayerPrefs.SetString(inventoryKey, inventoryJson);
            }
        }

        public object RestoreState()
        {
            string partyJson = PlayerPrefs.GetString(partyKey);
            if (string.IsNullOrEmpty(partyJson)) return null;

            List<PokemonSaveData> saveData = JsonConvert.DeserializeObject<List<PokemonSaveData>>(partyJson);
            string dexJson = PlayerPrefs.GetString(dexKey);
            if (!string.IsNullOrEmpty(dexJson))
            {
                pokedex = JsonConvert.DeserializeObject<HashSet<string>>(dexJson);
            }
            else
            {
                pokedex = new HashSet<string>();
            }
            string inventoryJson = PlayerPrefs.GetString(inventoryKey);
            if (!string.IsNullOrEmpty(inventoryJson))
            {
                List<PokemonSaveData> inventorySaveData = JsonConvert.DeserializeObject<List<PokemonSaveData>>(inventoryJson);
                inventory = inventorySaveData.Select(s => new PokemonUnit(s)).ToList();
            }
            return saveData;
        }
        public void PostEventAddPkmon(string pokemon)
        {
            Observer.Instance.Broadcast(EventId.OnAddPokemon, pokemon);
        }
        public void AddPkmToDex(PokemonUnit pokemon)
        {
            pokedex.Add(pokemon.Data.pokemonName);
            PostEventAddPkmon(pokemon.Data.pokemonName);
        }
    }
}