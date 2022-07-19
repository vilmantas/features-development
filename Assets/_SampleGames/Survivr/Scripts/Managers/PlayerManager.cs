using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerManager : MonoBehaviour, IManager
    {
        public LevelManager LevelManager;

        public CharacterController Player;
        
        public void Initialize()
        {
            LevelManager = GameObject.FindGameObjectWithTag(nameof(LevelManager))
                .GetComponent<LevelManager>();

            Player = LevelManager.InstantiatePlayer();
        }
    }
}