using UnityEngine;
using UnityEngine.EventSystems;

namespace _SampleGames.Survivr
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool GamePaused => Time.timeScale == 0f;

        public bool GameRunning => !GamePaused;
        
        [HideInInspector] public SceneLoader SceneLoader;
        
        private void Awake()
        {
            Instance = this;
            
            SceneLoader = FindObjectOfType<SceneLoader>();
            
            SceneLoader.Initialize();

            DontDestroyOnLoad(EventSystem.current.gameObject);
            
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMenu()
        {
            SceneLoader.LoadMenu();
        }

        public void LoadGame()
        {
            SceneLoader.LoadGame();
        }

        public void TogglePause()
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            
            SceneLoader.LoadPause();
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            
            SceneLoader.UnloadPause();
        }
    }
}