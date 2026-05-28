using System;
using System.Collections.Generic;
using DG.Tweening;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.Tutorial;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pokemon.Scripts.Battle
{
    public class MainPanel : MonoBehaviour
    {
        [SerializeField] RectTransform btnContainer;
        [SerializeField] Vector3 btnStartPos;
        [SerializeField] Button[] skillButtons;
        [SerializeField] BattleController battleController;
        Tween btnContainerTween;
        void OnDisable()
        {
            ClearButtonsEventListeners();
            btnContainer.DOKill();
        }
        public void EnablePanel(float duration, Action onComplete)
        {
            btnContainerTween?.Kill();
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnContainer.anchoredPosition.y - 265);
            btnContainerTween = btnContainer.DOAnchorPosY(btnStartPos.y, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        public void DisablePanel(float duration, Action onComplete)
        {
            btnContainerTween?.Kill();
            btnContainer.anchoredPosition = new Vector3(btnContainer.anchoredPosition.x, btnStartPos.y);
            btnContainerTween = btnContainer.DOAnchorPosY(btnContainer.anchoredPosition.y - 265, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        public void SetSkillButton(List<Skill> skills)
        {
            ClearButtonsEventListeners();
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (i < skills.Count)
                {
                    skillButtons[i].gameObject.SetActive(true);
                    skillButtons[i].GetComponent<Image>().sprite = skills[i].Data.icon;
                    int index = i;
                    skillButtons[i].onClick.AddListener(() =>
                     {
                         if (TutorialManager.Instance != null)
                         {
                             if (TutorialManager.Instance.btnIndex >= 0 && TutorialManager.Instance.btnIndex != index)
                             {
                                 Observer.Instance.Broadcast(EventId.OnShowMessage, "You must complete the tutorial step first!");
                                 return;
                             }

                         }
                         battleController.SetCurrentMove(index);
                     });
                }
                else
                {
                    skillButtons[i].gameObject.SetActive(false);
                }
            }
        }
        public void ClearButtonsEventListeners()
        {
            foreach (var btn in skillButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
    }
}