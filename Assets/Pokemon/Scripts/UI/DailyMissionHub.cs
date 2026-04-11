using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class DailyMissionHub : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI missionDescription;
        [SerializeField] private TextMeshProUGUI missionProgress;
        [SerializeField] private Button claimBtn;
        [SerializeField] private TextMeshProUGUI goldRewardText;

        public void InitHub(Quest.Quest quest)
        {
            missionDescription.text = quest.QuestData.description;
            missionProgress.text = $"{quest.CurrentCount}/{quest.QuestData.countToComplete}";
            claimBtn.interactable = !quest.IsClaimed;
            goldRewardText.text = $"{quest.QuestData.rewardMoney}";
        }
    }
}