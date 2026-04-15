using System;
using Pokemon.Scripts.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class EnterBattleScreen : ScreenBase
    {
        [SerializeField] private Button fightBtn;
        [SerializeField] private Image npcImage;
        [SerializeField] private TextMeshProUGUI npcNameText;
        [SerializeField] private TextMeshProUGUI npcMessageText;
        [SerializeField] private GameObject[] npcPokemon;
        public void Initialize(Action onFightBtnClick, NPC npc)
        {
            npcImage.sprite = npc.npcData.npcSprite;
            npcNameText.text = npc.npcData.npcName;
            npcMessageText.text = npc.npcData.npcMessage;
            int pokemonCount = npc.party.PokemonParties.Count;
            for (int i = 0; i < npcPokemon.Length; i++)
            {
                if (i < pokemonCount)
                {
                    npcPokemon[i].SetActive(true);

                }
                else
                {
                    npcPokemon[i].SetActive(false);
                }

            }
            base.Active();
            fightBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                ScreenManager.Instance.ActiveSplashScreen(onFightBtnClick);
            });
        }
        void OnDisable()
        {
            fightBtn.onClick.RemoveAllListeners();
        }
    }
}