using System;
using Codice.Client.BaseCommands.Differences;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public SceneLoader SceneLoader;
        
        private void Awake()
        {
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
    }
}