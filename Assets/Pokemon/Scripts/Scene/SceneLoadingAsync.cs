using System.Collections;
using Pokemon.Scripts.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pokemon.Scripts.Scene
{
    public class SceneLoadingAsync : MonoBehaviour
    {
        [SerializeField] private Image slider;
        [SerializeField] private TextMeshProUGUI textProgress;

        [SerializeField] private float minimumLoadingTime = 1f;

        private void Start()
        {
            StartCoroutine(LoadSceneAsync(Loader.targetScene));
        }

        IEnumerator LoadSceneAsync(Loader.Scene sceneName)
        {
            if (sceneName == Loader.Scene.LoadingScene)
            {
                sceneName = TutorialManager.IsTutorialCompleted()
                    ? Loader.Scene.GameScene
                    : Loader.Scene.TutorialScene;
            }

            Debug.Log(sceneName);

            float timer = 0f;

            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName.ToString());

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f || timer < minimumLoadingTime)
            {
                timer += Time.deltaTime;

                float progress =
                    Mathf.Clamp01(operation.progress / 0.9f);

                // Smooth progress theo thời gian tối thiểu
                float timeProgress =
                    Mathf.Clamp01(timer / minimumLoadingTime);

                float finalProgress =
                    Mathf.Min(progress, timeProgress);

                slider.fillAmount = finalProgress;

                textProgress.text =
                    Mathf.RoundToInt(finalProgress * 100f) + "%";

                yield return null;
            }

            slider.fillAmount = 1f;
            textProgress.text = "100%";

            yield return new WaitForSeconds(0.2f);

            operation.allowSceneActivation = true;

            Loader.targetScene = Loader.Scene.LoadingScene;
        }
    }
}