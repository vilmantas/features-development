using System.Linq;
using _SampleGames.Survivr.Scripts;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : Manager
    {
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

            if (!levelLoaded && !gameplayLoaded && !uiLoaded)
            {
                SceneManager.sceneLoaded += Initialized;

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

                if (!uiLoaded)
                {
                    SceneManager.LoadSceneAsync("_SampleGames/Survivr/Scenes/UI", LoadSceneMode.Additive);
                }
            }
        }

        private void Initialized(Scene arg0, LoadSceneMode arg1)
        {
            InitializeScene(arg0);
        }

        public void LoadGame()
        {
            SceneManager.sceneLoaded += OnGameStartSceneLoad;

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

        private void OnGameStartSceneLoad(Scene arg0, LoadSceneMode arg1)
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

            SceneManager.UnloadSceneAsync("Start");

            SceneManager.sceneLoaded -= OnGameStartSceneLoad;
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