using System;
using System.Collections;
using Pokemon.Scripts.MyUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon.Scripts.Scene
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public void Load(string scene)
        {
            StartCoroutine(LoadRoutine(scene));
        }

        IEnumerator LoadRoutine(string scene)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(scene);

            while (!op.isDone)
            {
                yield return null;
            }
        }
    }
}