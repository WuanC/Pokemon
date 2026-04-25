using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Pokemon
{
    public class PlayerParty : Party, ISavable
    {
        private const string partyKey = "PokemonParty";
        private const string inventoryKey = "PokemonInventory";

        public List<PokemonUnit> inventory { get; private set; }
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
        }

        public void CaptureState()
        {
            List<PokemonSaveData> partySaveData = PokemonParties.Select(p => p.GetSaveData()).ToList();
            string partyJson = JsonConvert.SerializeObject(partySaveData, Formatting.Indented);
            PlayerPrefs.SetString(partyKey, partyJson);
        }

        public object RestoreState()
        {
            string partyJson = PlayerPrefs.GetString(partyKey);
            if (string.IsNullOrEmpty(partyJson)) return null;

            return JsonConvert.DeserializeObject<List<PokemonSaveData>>(partyJson);
        }
    }
}