using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.LoadingScene
{
    public class LoadingManager : SingletonManager<LoadingManager>
    {
        protected override void DoSetup()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScenes(string[] scenes, string activeScene = "")
        {
            StartCoroutine(ShowLoadingScreen(scenes, activeScene));
        }

        private static IEnumerator ShowLoadingScreen(string[] scenes, string activeScene)
        {
            var op = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

            while (!op.isDone)
            {
                yield return null;
            }

            PrepareScene(scenes, activeScene);
        }

        private static void PrepareScene(string[] scenes, string activeScene)
        {
            var g = GameObject.Find("LoadingController").GetComponent<LoadingController>();

            g.Initialize(scenes, activeScene);
        }
    }
}