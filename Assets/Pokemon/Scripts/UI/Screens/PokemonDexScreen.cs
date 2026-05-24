using System.Collections.Generic;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PokemonDexScreen : ScreenBase
    {
        [SerializeField] private PokemonIcon pokemonIconPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private PlayerParty playerParty;
        private List<PokemonData> pokemonDatas;
        private List<PokemonIcon> pokemonIcons = new List<PokemonIcon>();
        protected override void Start()
        {
            base.Start();
            Observer.Instance.Register(EventId.OnAddPokemon, OnAddPokemon);
            Initialize();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Observer.Instance.Unregister(EventId.OnAddPokemon, OnAddPokemon);
        }
        public void Initialize()
        {
            pokemonDatas = PokemonDB.GetAllPokemon();
            foreach (var pkmData in pokemonDatas)
            {
                var icon = Instantiate(pokemonIconPrefab, content);
                icon.SetIcon(pkmData.icon, pkmData.pokemonName);
                icon.SetStatus(false);
                pokemonIcons.Add(icon);
            }
            foreach (var caughtPkm in playerParty.pokedex)
            {
                var icon = GetPokemonIconByName(caughtPkm);
                if (icon != null)
                {
                    icon.SetStatus(true);
                }
            }
        }
        public PokemonIcon GetPokemonIconByName(string pokemonName)
        {
            foreach (var icon in pokemonIcons)
            {
                if (icon.PokemonName == pokemonName)
                {
                    return icon;
                }
            }
            return null;
        }
        public void OnAddPokemon(object pokemonNameObj)
        {
            if (pokemonNameObj is string pokemonName)
            {
                var pkmIcon = GetPokemonIconByName(pokemonName);
                if (pkmIcon != null)
                {
                    pkmIcon.SetStatus(true);
                }
            }
        }


    }
}