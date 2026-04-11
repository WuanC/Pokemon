
using System.Collections.Generic;
using Pokemon.Scripts.MyUtils.ObjectPooling;
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PokemonDetailScreen : MonoBehaviour
    {
        [SerializeField] private Image pkmImage;
        [SerializeField] private PokemonModal pkmModal;
        [SerializeField] private TextMeshProUGUI pkmDescriptionText;
        [SerializeField] private CompatibleType compatibleType;
        [SerializeField] private Transform skillParent;
        [SerializeField] private SkillLocker skillIcon;
        List<SkillLocker> skillLockers = new();
        public void Initialize(PokemonUnit pokemonUnit)
        {
            pkmImage.sprite = pokemonUnit.Data.frontSprite;
            pkmModal.InitModal(pokemonUnit, true);
            pkmDescriptionText.text = pokemonUnit.Data.pokemonDescription;
            compatibleType.SetupCompatibleType(pokemonUnit.Data.type);
            for (int i = 0; i < pokemonUnit.Data.learnableSkills.Count; i++)
            {
                GameObject skillLockerGO = MyPoolManager.Instance.GetFromPool(skillIcon.gameObject, skillParent);
                skillLockerGO.transform.SetSiblingIndex(i);
                SkillLocker skillLocker = skillLockerGO.GetComponent<SkillLocker>();
                skillLocker.Initialize(pokemonUnit, pokemonUnit.Data.learnableSkills[i]);
                skillLockers.Add(skillLocker);
            }
        }
        void OnDisable()
        {
            foreach (var skill in skillLockers)
            {
                skill.gameObject.SetActive(false);
            }
            skillLockers.Clear();
        }

    }
}