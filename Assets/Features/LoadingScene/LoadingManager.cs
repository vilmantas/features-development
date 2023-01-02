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

        public void LoadAdditionalScenes(params string[] scenes)
        {
            StartCoroutine(LoadLoadingScene(scenes));
        }

        private static IEnumerator LoadLoadingScene(string[] scenes)
        {
            var op = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

            while (!op.isDone)
            {
                yield return null;
            }

            PrepareScene(scenes);
        }

        private static void PrepareScene(string[] scenes)
        {
            var g = GameObject.Find("LoadingController").GetComponent<LoadingController>();

            g.Initialize(scenes);
        }
    }
}