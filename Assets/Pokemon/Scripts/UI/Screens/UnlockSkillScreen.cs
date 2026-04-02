
using System.Collections;
using DG.Tweening;
using Pokemon.Scripts.Pokemon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class UnlockSkillScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textUnlockNewSkill;
        [SerializeField] private Image skillImage;
        [SerializeField] private Animator imageAnimator;
        [SerializeField] private TextMeshProUGUI textDescriptionSkill;

        [SerializeField] private GameObject forgetPanel;
        [SerializeField] private TextMeshProUGUI textForgetSkill;
        [SerializeField] private Button[] skillButtons;
        [SerializeField] private Button skipForgetBtn;

        [SerializeField] private Button okBtn;
        private bool isExitPanel = false;
        private bool selectSkillToForget = false;

        void OnDisable()
        {
            textUnlockNewSkill.transform.DOKill();
        }
        void Start()
        {
            okBtn.onClick.AddListener(() =>
            {
                imageAnimator.enabled = true;
                gameObject.SetActive(false);
                isExitPanel = true;
            });
        }
        public IEnumerator UnlockNewSkill(bool isForgettingSkill, SkillData newSKillData, PokemonUnit pokemonUnit = null)
        {
            Init();
            textUnlockNewSkill.transform.localScale = Vector3.zero;
            yield return textUnlockNewSkill.transform.DOScale(Vector3.one * 1.2f, 0.5f).WaitForCompletion();
            skillImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.17f);
            imageAnimator.enabled = false;
            skillImage.sprite = newSKillData.icon;
            textDescriptionSkill.text = $"{newSKillData.description}";
            textDescriptionSkill.gameObject.SetActive(true);
            if (isForgettingSkill)
            {
                for (int i = 0; i < skillButtons.Length; i++)
                {
                    int index = i;
                    skillButtons[i].GetComponent<Image>().sprite = pokemonUnit.Skills[i].Data.icon;
                    skillButtons[i].onClick.AddListener(() =>
                    {
                        pokemonUnit.ReplaceSkill(index, new Skill(newSKillData));
                        skillButtons[index].GetComponent<Image>().sprite = newSKillData.icon;
                        selectSkillToForget = true;
                        RemoveForgetSkillListeners();
                    });
                }
                skipForgetBtn.onClick.AddListener(() =>
                {
                    selectSkillToForget = true;
                    RemoveForgetSkillListeners();
                });
                forgetPanel.SetActive(true);
                yield return new WaitUntil(() => selectSkillToForget);
            }
            okBtn.gameObject.SetActive(true);
            yield return new WaitUntil(() => isExitPanel);

        }
        public void Init()
        {
            isExitPanel = false;
            selectSkillToForget = false;
            forgetPanel.SetActive(false);
            textDescriptionSkill.gameObject.SetActive(false);
            okBtn.gameObject.SetActive(false);
            skillImage.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        public void RemoveForgetSkillListeners()
        {
            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].onClick.RemoveAllListeners();
            }
            skipForgetBtn.onClick.RemoveAllListeners();
        }
    }
}