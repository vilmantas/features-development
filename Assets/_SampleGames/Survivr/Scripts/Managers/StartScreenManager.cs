using UnityEngine;
using UnityEngine.UI;

namespace _SampleGames.Survivr
{
    public class StartScreenManager : Manager
    {
        private Button StartButton;
        
        public override void Initialize()
        {
            StartButton = FindObjectOfType<Button>();

            var manager = FindObjectOfType<GameManager>();
            
            StartButton.onClick.AddListener(manager.LoadGame); 
        }
    }
}