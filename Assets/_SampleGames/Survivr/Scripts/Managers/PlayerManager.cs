using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerManager : Manager
    {
        [HideInInspector] public LevelManager LevelManager;

        [HideInInspector] public CharacterController Player;
        
        public override void Initialize()
        {
            LevelManager = GameObject.FindGameObjectWithTag(nameof(LevelManager))
                .GetComponent<LevelManager>();

            Player = LevelManager.InstantiatePlayer();
        }
    }
}