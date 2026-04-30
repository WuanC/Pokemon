using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class SkillDetailScreen : MonoBehaviour
    {
        [SerializeField] private int relearnPrice;
        [SerializeField] private PokemonDetailScreen pokemonDetailScreen;
        [Header("Skill info")]
        [SerializeField] private Image skillIcon;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillDescriptionText;
        [SerializeField] private TextMeshProUGUI skillPowerText;
        [SerializeField] private TextMeshProUGUI skillPkmTypeText;

        [Header("Relearn")]
        [SerializeField] private Sprite relearnIconDefault;
        [SerializeField] private GameObject relearnPanel;
        [SerializeField] private Button choseSkillButton;
        [SerializeField] private Image currentSkillIcon;
        [SerializeField] private Image relearnIcon;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button confirmRelearnButton;
        [Header("Panel pokemon current skill")]
        [SerializeField] private GameObject currentSkillPanel;
        [SerializeField] private Button[] pokemonSkill;
        private int indexCurrentSkillSelected;


        public void Initialize(PokemonUnit pkmUnit, SkillData skillData, bool canRelearn = false)
        {
            gameObject.SetActive(true);
            skillIcon.sprite = skillData.icon;
            skillNameText.text = $"Name: {skillData.skillName}";
            skillDescriptionText.text = skillData.description;
            skillPowerText.text = $"Power: {skillData.power}";
            skillPkmTypeText.text = $"Type: {skillData.elementType.ToString()}";
            relearnPanel.SetActive(canRelearn);
            if (canRelearn)
            {
                indexCurrentSkillSelected = -1;
                currentSkillIcon.sprite = skillData.icon;
                relearnIcon.sprite = relearnIconDefault;
                priceText.text = $"{relearnPrice}";

                choseSkillButton.onClick.AddListener(() =>
                {
                    currentSkillPanel.SetActive(true);
                    for (int i = 0; i < pokemonSkill.Length; i++)
                    {
                        if (i < pkmUnit.Skills.Count)
                        {
                            pokemonSkill[i].gameObject.SetActive(true);
                            pokemonSkill[i].GetComponent<Image>().sprite = pkmUnit.Skills[i].Data.icon;
                            int index = i;
                            pokemonSkill[i].onClick.AddListener(() =>
                            {
                                indexCurrentSkillSelected = index;
                                relearnIcon.sprite = pkmUnit.Skills[index].Data.icon;
                                ClearEventCurrentSkills();
                                currentSkillPanel.SetActive(false);
                            });
                        }
                        else
                        {
                            pokemonSkill[i].gameObject.SetActive(false);
                        }
                    }
                });
                confirmRelearnButton.onClick.AddListener(() =>
                {
                    if (indexCurrentSkillSelected == -1)
                    {
                        Observer.Instance.Broadcast(EventId.OnShowMessage, "Please select a skill to relearn");
                        return;
                    }
                    if (Inventory.Inventory.Instance.CanPayDust(relearnPrice))
                    {
                        Observer.Instance.Broadcast(EventId.OnShowMessage, "Relearned " + skillData.skillName + " replacing " + pkmUnit.Skills[indexCurrentSkillSelected].Data.skillName);
                        Inventory.Inventory.Instance.PayDust(relearnPrice);
                        pkmUnit.ReplaceSkill(indexCurrentSkillSelected, new Skill(skillData));
                        gameObject.SetActive(false);
                        pokemonDetailScreen.ReloadSkill(pkmUnit);
                        return;
                    }
                    else
                    {
                        Observer.Instance.Broadcast(EventId.OnShowMessage, "You don't have enough dust to relearn this skill");
                    }
                });
            }
        }
        public void ClearEventCurrentSkills()
        {
            for (int i = 0; i < pokemonSkill.Length; i++)
            {
                pokemonSkill[i].onClick.RemoveAllListeners();
            }
        }
        void OnDisable()
        {
            choseSkillButton.onClick.RemoveAllListeners();
            confirmRelearnButton.onClick.RemoveAllListeners();
        }
    }
}