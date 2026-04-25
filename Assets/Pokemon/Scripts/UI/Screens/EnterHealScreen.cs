using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class EnterHealScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI npcNameText;
        [SerializeField] private TextMeshProUGUI npcMessageText;
        [SerializeField] private Image npcAvt;
        [SerializeField] private Button healBtn;
        [SerializeField] private List<Image> pokemonImages;
        [SerializeField] private Image healProgress;
        Tween healTween;
        public void Initialize(List<PokemonUnit> pokemons, NPCHeal npc)
        {
            healProgress.transform.parent.gameObject.SetActive(false);
            healBtn.interactable = true;
            npcAvt.sprite = npc.npcData.npcSprite;
            npcNameText.text = npc.npcData.npcName;
            npcMessageText.text = npc.npcData.npcMessage;
            for (int i = 0; i < pokemonImages.Count; i++)
            {
                if (i < pokemons.Count)
                {
                    pokemonImages[i].sprite = pokemons[i].Data.icon;
                    pokemonImages[i].gameObject.SetActive(true);
                }
                else
                {
                    pokemonImages[i].gameObject.SetActive(false);
                }
            }
            healBtn.onClick.AddListener(() =>
            {
                Heal(npc, pokemons);
            });
            base.Active();
        }
        public void Heal(NPCHeal npc, List<PokemonUnit> pokemons)
        {
            healBtn.interactable = false;
            healProgress.transform.parent.gameObject.SetActive(true);
            healProgress.fillAmount = 0;
            healTween = healProgress.DOFillAmount(1, 5f).OnComplete(() =>
            {
                healProgress.transform.parent.gameObject.SetActive(false);
                npc.Heal(pokemons);
                healBtn.interactable = true;
                Observer.Instance.Broadcast(EventId.OnShowMessage, "Heal complete!");
            });
        }
        public override void Deactive()
        {
            base.Deactive();
            healBtn.onClick.RemoveAllListeners();
            healTween.Kill();
        }
    }
}