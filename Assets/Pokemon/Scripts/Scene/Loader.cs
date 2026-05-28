using System;
using System.Collections;
using Pokemon.Scripts.MyUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon.Scripts.Scene
{
    public static class Loader
    {
        public static Scene targetScene;
        public enum Scene
        {
            LoadingScene,
            GameScene,
            TutorialScene,
        }
        public static void LoadScene(Loader.Scene scene)
        {
            targetScene = scene;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

    }
}