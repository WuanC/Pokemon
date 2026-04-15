using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PlayScreen : MonoBehaviour
    {
        [SerializeField] private Button worldBtn;
        [SerializeField] private TextMeshProUGUI coinText;
        public Transform coinTransform;
        void Start()
        {
            Initialize();
            worldBtn.gameObject.SetActive(false);
        }
        public void Initialize()
        {
            worldBtn.onClick.AddListener(() =>
            {
                ScreenManager.Instance.ActiveSplashScreen(() =>
                {
                    GameController.Instance.BackToWorldMap();
                    worldBtn.gameObject.SetActive(false);
                });
            });
        }
        public void EnterDetailMap()
        {
            worldBtn.gameObject.SetActive(true);
        }
        public void UpdateCoinText(int coins)
        {
            coinText.text = coins.ToString();
        }
    }
}