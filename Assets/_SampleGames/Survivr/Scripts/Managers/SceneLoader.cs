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
        private void Start()
        {
            // SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            //
            // SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Level", LoadSceneMode.Additive);
            // SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Lighting", LoadSceneMode.Additive);
            // SceneManager.LoadScene("_SampleGames/Survivr/Scenes/Gameplay", LoadSceneMode.Additive);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level"));
            
            InitializeScene("Level");
            
            InitializeScene("Gameplay");
        }

        private static void InitializeScene(string name)
        {
            var initializer = SceneManager.GetSceneByName(name).GetRootGameObjects()
                .First(x => x.GetComponent<SceneInitializer>() != null).GetComponent<SceneInitializer>();

            initializer.StartScene();
        }
    }
}