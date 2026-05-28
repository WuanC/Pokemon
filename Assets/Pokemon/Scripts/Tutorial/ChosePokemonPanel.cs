using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.Tutorial
{
    public class ChosePokemonPanel : MonoBehaviour
    {
        private ChosePokemonScreen chosePokemonScreen;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image bgImage;
        [SerializeField] private Image pkmImage;
        [SerializeField] private Button choseBtn;

        [SerializeField] private Sprite fireBg;
        [SerializeField] private Sprite waterBg;
        [SerializeField] private Sprite grassBg;

        public void InitPanel(PokemonParty pkmParty, ChosePokemonScreen screen)
        {
            chosePokemonScreen = screen;
            nameText.text = pkmParty.pokemonData.pokemonName;
            switch (pkmParty.pokemonData.type)
            {
                case PkmType.Fire:
                    bgImage.sprite = fireBg;
                    break;
                case PkmType.Water:
                    bgImage.sprite = waterBg;
                    break;
                case PkmType.Earth:
                    bgImage.sprite = grassBg;
                    break;
            }
            pkmImage.sprite = pkmParty.pokemonData.frontSprite;
            pkmImage.SetNativeSize();
            choseBtn.onClick.AddListener(() => OnChose(pkmParty));
        }

        void OnChose(PokemonParty pkmParty)
        {
            PokemonUnit pkmUnit = new PokemonUnit(pkmParty.pokemonData, 5);
            PlayerParty.Instance.Initialize();
            PlayerParty.Instance.AddPokemon(pkmUnit);
            chosePokemonScreen.DisableScreen();
        }
        void OnDestroy()
        {
            choseBtn.onClick.RemoveAllListeners();
        }
    }
}