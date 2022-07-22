using UnityEngine;

namespace _SampleGames.Survivr.Scripts
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField]
        private Manager[] Managers;

        public void StartScene()
        {
            foreach (var manager in Managers)
            {
                manager.Initialize();
            }
        }
    }
}