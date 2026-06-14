using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.WDebug
{
    public class DebugController : MonoBehaviour
    {
        [SerializeField] private Button debugBtn;

        [SerializeField] private GameObject debugPasswordPanel;
        [SerializeField] private Button debugPasswordSubmitBtn;
        [SerializeField] private TMP_InputField debugPasswordInputField;
        [SerializeField] private string debugPassword = "123123";


        [SerializeField] private GameObject mainDebugScreen;

        private bool isUnlock = false;
        [SerializeField] private int debugUnlockCount = 0;
        [SerializeField] private float lastTimeClicked = 0f;

        private void Start()
        {
            debugBtn.onClick.AddListener(OnDebugButtonClicked);
            debugPasswordSubmitBtn.onClick.AddListener(OnDebugPasswordSubmit);
        }
        private void OnDestroy()
        {
            debugBtn.onClick.RemoveAllListeners();
            debugPasswordSubmitBtn.onClick.RemoveAllListeners();
        }

        private void OnDebugButtonClicked()
        {
            float deltaClickTime = Time.time - lastTimeClicked;
            if (deltaClickTime > 1f)
            {
                debugUnlockCount = 1;
            }
            else
            {
                debugUnlockCount++;
            }
            lastTimeClicked = Time.time;
            if (debugUnlockCount <= 5) return;
            if (isUnlock)
            {
                mainDebugScreen.SetActive(true);
                return;
            }
            else
            {

                debugPasswordPanel.SetActive(true);
            }
        }
        private void OnDebugPasswordSubmit()
        {
            Debug.Log("Debug password submitted: " + debugPasswordInputField.text + " " + debugPassword); ;
            if (debugPasswordInputField.text == debugPassword)
            {
                isUnlock = true;
                mainDebugScreen.SetActive(true);
                debugPasswordPanel.SetActive(false);
            }
            else
            {
                debugPasswordInputField.text = "";
            }
        }
    }
}