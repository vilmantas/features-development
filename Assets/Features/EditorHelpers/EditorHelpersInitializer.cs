using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.EditorHelpers
{
    public static class EditorHelpersInitializer
    {
        public static string[] RequiredGameplayScenes = {"Character_Main", "Gameplay", "Lighting", "Gameplay_UI"};
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeAllRequirements()
        {
            #if !UNITY_EDITOR
                return;
            #endif
            
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                Debug.Log(scene.name);
            }
        }
    }
}