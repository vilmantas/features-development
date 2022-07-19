using System;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class LevelManager : MonoBehaviour, IManager
    {
        public Action LevelInitialized;

        public CharacterController Player;
        
        public EnemyController[] LevelEnemies;

        [Range(1, 10)] public float SpawnIntervalSeconds;

        public Transform Spawn;
        
        public void Initialize()
        {
            Spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        }

        public CharacterController InstantiatePlayer()
        {
            return Instantiate(Player, Spawn.position, Quaternion.identity);
        }
    }
}