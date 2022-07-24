using System;
using System.Collections.Generic;
using System.Linq;
using _SampleGames.Survivr.Scripts;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : Manager
    {

        private readonly Dictionary<string, bool> WaitingForScenes = new();
        
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
            
            SetActiveSceneAfterLoad();

            UnloadSceneAfterLoad();
            
            InitializeWaitingScenes();
            
            ResetWaitingMethod();
        }

        private void ResetWaitingMethod()
        {
            UnloadScene = string.Empty;

            SetActiveScene = string.Empty;

            WaitingForScenes.Clear();

            SceneManager.sceneLoaded -= WaitForScenes;
        }

        private void InitializeWaitingScenes()
        {
            foreach (var waitForScene in WaitingForScenes)
            {
                InitializeScene(waitForScene.Key);
            }
        }

        private void UnloadSceneAfterLoad()
        {
            if (!string.IsNullOrEmpty(UnloadScene))
            {
                var unloadScene = SceneManager.GetSceneByName(UnloadScene);

                if (unloadScene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(unloadScene);
                }
            }
        }

        private void SetActiveSceneAfterLoad()
        {
            if (!string.IsNullOrEmpty(SetActiveScene))
            {
                var activeScene = SceneManager.GetSceneByName(SetActiveScene);

                SceneManager.SetActiveScene(activeScene);
            }
        }

        public void LoadGame()
        {
            WaitingForScenes.Add("Level", false);
            WaitingForScenes.Add("Gameplay", false);
            WaitingForScenes.Add("UI", false);
            
            SceneManager.sceneLoaded += WaitForScenes;

            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Level");
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/UI", LoadSceneMode.Additive);
        }

        public void LoadMenu()
        {
            WaitingForScenes.Add("Start", false);
            
            SceneManager.sceneLoaded += WaitForScenes;

            SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Start");
        }

        private static void InitializeScene(string name)
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