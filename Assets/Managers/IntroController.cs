using UnityEngine;

namespace Managers
{
    public class IntroController : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }
    }
}