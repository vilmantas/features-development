using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _SampleGames.Survivr
{
    public class SceneLoader : MonoBehaviour
    {
        private void Start()
        {
            // SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
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
                var manager = rootGameObject.GetComponent<IManager>();

                manager?.Initialize();
            }
        }
    }
}