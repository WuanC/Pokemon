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
    public class Party : MonoBehaviour, ISavable
    {
        private const string partyKey = "PokemonParty";
        private const string inventoryKey = "PokemonInventory";
        [SerializeField] PokemonParty[] initPokemonParties;
        public List<PokemonUnit> PokemonParties { get; private set; }
        public List<PokemonUnit> inventory { get; private set; }
        public void Initialize()
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

        public void InitParty()
        {

            for (int i = 0; i < initPokemonParties.Length; i++)
            {
                PokemonUnit pokemonUnit = new PokemonUnit(initPokemonParties[i].pokemonData, initPokemonParties[i].level);
                PokemonParties.Add(pokemonUnit);
            }
        }
        public PokemonUnit GetHealthyPokemon()
        {
            return PokemonParties.FirstOrDefault(p => p.HP > 0);
        }
        public void AddPokemon(PokemonUnit pokemon)
        {
            if (PokemonParties.Count < 3)
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