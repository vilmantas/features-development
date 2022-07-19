using System;
using System.Collections.Generic;
using System.Linq;
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
            
            InitializeScene("Level");
            
            InitializeScene("Gameplay");
        }

        private static void InitializeScene(string name)
        {
            foreach (var rootGameObject in SceneManager.GetSceneByName(name).GetRootGameObjects())
            {
                var managers = new List<IManager>();
                
                managers.AddRange(rootGameObject.GetComponentsInChildren<IManager>());

                foreach (var manager in managers)
                {
                    manager?.Initialize();
                }
            }
        }
    }
}