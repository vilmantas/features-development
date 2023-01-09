using System;
using UnityEngine;

namespace Managers
{
    public class IntroController : MonoBehaviour
    {
        public bool SkipIntro;
        
        private void Start()
        {
#if UNITY_EDITOR
            if (!SkipIntro) return;
            
            GameManager.Instance.StartGame();
#endif
        }

        public void StartGame()
        {

            
        }
    }
}