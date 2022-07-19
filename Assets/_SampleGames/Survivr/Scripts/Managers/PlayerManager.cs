using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerManager : MonoBehaviour, IManager
    {
        [HideInInspector] public LevelManager LevelManager;

        [HideInInspector] public CharacterController Player;
        
        public void Initialize()
        {
            LevelManager = GameObject.FindGameObjectWithTag(nameof(LevelManager))
                .GetComponent<LevelManager>();

            Player = LevelManager.InstantiatePlayer();
        }
    }
}