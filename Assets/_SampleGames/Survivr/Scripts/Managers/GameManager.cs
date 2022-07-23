using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class GameManager : MonoBehaviour
    {
        public SceneLoader SceneLoader;
        
        private void Awake()
        {
            SceneLoader = FindObjectOfType<SceneLoader>();
            
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadMenu()
        {
            SceneLoader.LoadMenu();
        }
    }
}