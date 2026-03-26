using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class PlayScreen : MonoBehaviour
    {
        [SerializeField] private Button worldBtn;
        void Start()
        {
            Initialize();
            worldBtn.gameObject.SetActive(false);
        }
        public void Initialize()
        {
            worldBtn.onClick.AddListener(() =>
            {
                GameController.Instance.ActiveSplashScreen(() =>
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
    }
}