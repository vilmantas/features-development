using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerManager : Manager
    {
        [HideInInspector] public GameManager GameManager;
        
        [HideInInspector] public LevelManager LevelManager;

        [HideInInspector] public CharacterController Player;
        
        public override void Initialize()
        {
            LevelManager = GameObject.FindGameObjectWithTag(nameof(LevelManager))
                .GetComponent<LevelManager>();

            GameManager = GameObject.FindGameObjectWithTag(nameof(GameManager))
                .GetComponent<GameManager>();
            
            Player = LevelManager.InstantiatePlayer();

            Player.OnDeath += () => GameManager.LoadMenu();

        }
    }
}