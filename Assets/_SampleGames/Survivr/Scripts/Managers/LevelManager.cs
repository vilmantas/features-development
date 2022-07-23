using System;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class LevelManager : Manager
    {
        public CharacterController PlayerPrefab;
        
        public EnemyController[] LevelEnemies;

        [Range(1, 10)] public float SpawnIntervalSeconds;

        public Transform Spawn;
        
        public override void Initialize()
        {
            Spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        }

        public CharacterController InstantiatePlayer()
        {
            return Instantiate(PlayerPrefab, Spawn.position, Quaternion.identity);
        }
    }
}