using System;
using System.Collections.Generic;
using System.Linq;
using _SampleGames.Survivr.Scripts;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : Manager
    {

        private Dictionary<string, bool> WaitingForScenes = new();
        
        private string SetActiveScene = string.Empty;

        private string UnloadScene = string.Empty;

        public override void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            bool levelLoaded = false;
            bool gameplayLoaded = false;
            bool uiLoaded = false;

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

                if (scene.name.StartsWith("UI"))
                {
                    uiLoaded = true;
                }
            }

            SceneManager.sceneLoaded += WaitForScenes;

            UnloadScene = "__INIT";
            
            if (!levelLoaded && !gameplayLoaded && !uiLoaded)
            {
                WaitingForScenes.Add("Start", false);
                
                SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Start");
            }
            else
            {
                WaitingForScenes.Add("Level", levelLoaded);
                WaitingForScenes.Add("Gameplay", gameplayLoaded);
                WaitingForScenes.Add("UI", uiLoaded);
                
                SetActiveScene = "Level";
                
                if (!levelLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Level", LoadSceneMode.Additive);
                }

                if (!gameplayLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
                }

                if (!uiLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/UI", LoadSceneMode.Additive);
                }
            }
        }

        private void WaitForScenes(Scene scene, LoadSceneMode mode)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                
                if (!s.isLoaded) continue;

                if (WaitingForScenes.ContainsKey(s.name))
                {
                    WaitingForScenes[s.name] = true;
                }
            }

            if (!WaitingForScenes.All(x => x.Value)) return;
            
            foreach (var waitForScene in WaitingForScenes)
            {
                InitializeScene(waitForScene.Key);
            }
            
            WaitingForScenes.Clear();

            if (!string.IsNullOrEmpty(SetActiveScene))
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SetActiveScene));
            }

            if (!string.IsNullOrEmpty(UnloadScene))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(UnloadScene));
            }
            
            SceneManager.sceneLoaded -= WaitForScenes;
        }
        

        private void Initialized(Scene arg0, LoadSceneMode arg1)
        {
            InitializeScene(arg0);
            
            SceneManager.sceneLoaded -= Initialized;
        }

        public void LoadGame()
        {
            WaitingForScenes.Add("Level", false);
            WaitingForScenes.Add("Gameplay", false);
            WaitingForScenes.Add("UI", false);
            
            SceneManager.sceneLoaded += WaitForScenes;

            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Level", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/UI", LoadSceneMode.Additive);
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            bool levelLoaded = false;
            bool gameplayLoaded = false;
            bool uiLoaded = false;

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

                if (scene.name.StartsWith("UI"))
                {
                    uiLoaded = true;
                }
            }

            if (!levelLoaded || !gameplayLoaded || !uiLoaded) return;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level"));

            InitializeScene("Level");
            InitializeScene("Gameplay");
            InitializeScene("UI");

            SceneManager.UnloadSceneAsync("__INIT");

            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Start", LoadSceneMode.Additive);

            SceneManager.sceneLoaded += OnStartLoaded;
        }

        private void OnStartLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Start"));

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Level"));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Gameplay"));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("UI"));

            InitializeScene(arg0);

            SceneManager.sceneLoaded -= OnStartLoaded;
        }

        public static void InitializeScene(string name)
        {
            InitializeScene(SceneManager.GetSceneByName(name));
        }

        private static void InitializeScene(Scene scene)
        {
            var initializer = scene.GetRootGameObjects()
                .FirstOrDefault(x => x.GetComponent<SceneInitializer>() != null)?.GetComponent<SceneInitializer>();

            if (initializer == null) return;

            initializer.StartScene();
        }
    }
}