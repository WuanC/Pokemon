using System;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.FReward;
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
        [SerializeField] private RewardSlot[] rewardSlots;
        public void Initialize(Action onFightBtnClick, NPCBattle npc)
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
            InitializeReward(npc.reward);
            base.Active();
            fightBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                ScreenManager.Instance.ActiveSplashScreen(onFightBtnClick);
            });
        }
        public void InitializeReward(Reward reward)
        {
            for (int i = 0; i < rewardSlots.Length; i++)
            {
                if (i < reward.items.Count)
                {
                    rewardSlots[i].gameObject.SetActive(true);
                    rewardSlots[i].Initialize(reward.items[i].ItemBase.icon, reward.items[i].Quantity);
                }
                else
                {
                    rewardSlots[i].gameObject.SetActive(false);
                }
            }
        }
        void OnDisable()
        {
            fightBtn.onClick.RemoveAllListeners();
        }
    }
}