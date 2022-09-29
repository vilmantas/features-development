using System.Collections.Generic;
using System.Linq;
using _SampleGames.Survivr.Scripts;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : Manager
    {
        private readonly Dictionary<string, bool> WaitingForScenes = new();

        private static readonly string[] GameplayScenes = new[] {"Level", "Gameplay", "Game_UI"};
        
        private string SetActiveScene = "Level";

        private string UnloadScene = "__INIT";

        public override void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            var ignoreStart = AnyDevelopmentScenesLoaded();

            if (!ignoreStart)
            {
                WaitingForScenes.Add("Start", false);

                SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Start");
            }
            else
            {
                LoadMissingGameplayScenes();
            }

            SceneManager.sceneLoaded += WaitForScenes;
        }

        private void LoadMissingGameplayScenes()
        {
            var sceneLoading = false;

            foreach (var s in GameplayScenes)
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);

                    if (scene.name != s) continue;

                    sceneLoading = true;

                    break;
                }

                if (!sceneLoading)
                {
                    SceneManager.LoadSceneAsync($"_SampleGames/Survivr/Scenes/{s}", LoadSceneMode.Additive);
                }

                WaitingForScenes.Add(s, false);

                sceneLoading = false;
            }
        }

        private static bool AnyDevelopmentScenesLoaded()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (GameplayScenes.Any(x => scene.name.StartsWith(x)))
                {
                    return true;
                }
            }

            return false;
        }

        private void WaitForScenes(Scene scene, LoadSceneMode mode)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);

                if (!s.isLoaded) continue;

                if (WaitingForScenes.ContainsKey(s.name))
                {
                    WaitingForScenes[s.name] = true;
                }
            }

            if (!WaitingForScenes.All(x => x.Value)) return;

            SetActiveSceneSafe(SetActiveScene);

            UnloadSceneSafe(UnloadScene);

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

        private void UnloadSceneSafe(string scene)
        {
            if (string.IsNullOrEmpty(scene)) return;
            
            var unloadScene = SceneManager.GetSceneByName(scene);

            if (!unloadScene.IsValid()) return;
            
            if (unloadScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(unloadScene);
            }
        }

        private void SetActiveSceneSafe(string scene)
        {
            var activeScene = SceneManager.GetSceneByName(scene);

            if (!activeScene.IsValid()) return;
            
            SceneManager.SetActiveScene(activeScene);
        }

        public void LoadPause()
        {
            WaitingForScenes.Add("Pause_UI", false);

            SceneManager.sceneLoaded += WaitForScenes;

            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Pause_UI", LoadSceneMode.Additive);
        }

        public void UnloadPause()
        {
            UnloadSceneSafe("Pause_UI");
        }

        public void LoadGame()
        {
            WaitingForScenes.Add("Level", false);
            WaitingForScenes.Add("Gameplay", false);
            WaitingForScenes.Add("Game_UI", false);

            SceneManager.sceneLoaded += WaitForScenes;

            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Level");
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/Game_UI", LoadSceneMode.Additive);
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

            if (initializer != null)
            {
                initializer.StartScene();
            }
            else
            {
                var roots = scene.GetRootGameObjects();

                var managers = new List<Manager>();

                foreach (var root in roots)
                {
                    var children = root.GetComponentsInChildren<Manager>();

                    if (children.Any())
                    {
                        managers.AddRange(children);
                    }
                }

                foreach (var manager in managers)
                {
                    manager.Initialize();
                }
            }
        }
    }
}