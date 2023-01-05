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
            LoadingManager.Instance.LoadAdditionalScenes("Character_Main", "Scenes/Gameplay", "Scenes/Lighting");
        }
    }
}