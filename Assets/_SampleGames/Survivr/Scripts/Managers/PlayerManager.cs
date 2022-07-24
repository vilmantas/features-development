using UnityEngine;

namespace _SampleGames.Survivr
{
    public class PlayerManager : Manager
    {
        [HideInInspector] public GameManager GameManager;
        
        [HideInInspector] public LevelSceneManager LevelSceneManager;

        [HideInInspector] public CharacterController Player;
        
        public override void Initialize()
        {
            LevelSceneManager = GameObject.FindGameObjectWithTag(nameof(LevelSceneManager))
                .GetComponent<LevelSceneManager>();

            GameManager = GameObject.FindGameObjectWithTag(nameof(GameManager))
                .GetComponent<GameManager>();
            
            Player = LevelSceneManager.InstantiatePlayer();

            Player.OnDeath += () => GameManager.LoadMenu();

        }
    }
}