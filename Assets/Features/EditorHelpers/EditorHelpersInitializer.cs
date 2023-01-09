using System.Collections.Generic;
using System.Linq;
using Features.LoadingScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.EditorHelpers
{
    public static class EditorHelpersInitializer
    {
        public static string[] RequiredGameplayScenes = {"Character_Main", "Gameplay", "Lighting", "Gameplay_UI"};

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAllRequirements()
        {
            #if !UNITY_EDITOR
                return;
            #endif

            List<string> existingScenes = new();
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                existingScenes.Add(scene.name);
            }

            var missingScenes = RequiredGameplayScenes.Where(x => !existingScenes.Contains(x));
            
            LoadingManager.Instance.LoadScenes(missingScenes.ToArray(), "Lighting");
        }
    }
}