using System;
using System.Collections.Generic;
using System.Linq;
using _SampleGames.Survivr.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            bool levelLoaded = false;
            bool gameplayLoaded = false;
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene.name.StartsWith("Level"))
                {
                    levelLoaded = true;
                }

                if (scene.name.StartsWith("Gameplay"))
                {
                    gameplayLoaded = true;
                }
            }

            if (!levelLoaded && !gameplayLoaded)
            {
                SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Start");
            }
            else
            {
                SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
                
                if (!levelLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Level", LoadSceneMode.Additive);
                }

                if (!gameplayLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
                }
            }
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            bool levelLoaded = false;
            bool gameplayLoaded = false;
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                if (!scene.isLoaded) continue;
                
                if (scene.name.StartsWith("Level"))
                {
                    levelLoaded = true;
                }

                if (scene.name.StartsWith("Gameplay"))
                {
                    gameplayLoaded = true;
                }
            }

            if (!levelLoaded || !gameplayLoaded) return;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level"));
            
            InitializeScene("Level");
            InitializeScene("Gameplay");
            
            SceneManager.UnloadSceneAsync("__INIT");

            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Start", LoadSceneMode.Additive);

            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Start"));
                
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Level"));
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Gameplay"));
            
                InitializeScene("Start");
            };
        }

        public static void InitializeScene(string name)
        {
            var initializer = SceneManager.GetSceneByName(name).GetRootGameObjects()
                .FirstOrDefault(x => x.GetComponent<SceneInitializer>() != null)?.GetComponent<SceneInitializer>();

            if (initializer == null) return;
            
            initializer.StartScene();
        }
    }
}