using Features.LoadingScene;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
        protected override void DoSetup()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame()
        {
            var scenes = new[] {"Character_Main", "Scenes/Gameplay", "Scenes/Lighting"};
            
            LoadingManager.Instance.LoadScenes(scenes, "Scenes/Lighting");
        }
    }
}