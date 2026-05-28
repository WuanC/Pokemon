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

        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textProgress;
        private void Start()
        {
            StartCoroutine(LoadSceneAsync(Loader.targetScene));
        }


        IEnumerator LoadSceneAsync(Loader.Scene sceneName)
        {
            if (sceneName == Loader.Scene.LoadingScene)
            {
                if (TutorialManager.IsTutorialCompleted())
                {
                    sceneName = Loader.Scene.GameScene;
                }
                else
                {
                    sceneName = Loader.Scene.TutorialScene;
                }
            }
            Debug.Log(sceneName);
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName.ToString());

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                slider.value = progress;
                textProgress.text = (progress * 100f).ToString("F0") + "%";

                yield return null;
            }

            slider.value = 1f;
            textProgress.text = "100%";
            yield return new WaitForSeconds(0.3f);
            operation.allowSceneActivation = true;
            Loader.targetScene = Loader.Scene.LoadingScene;
        }

    }
}