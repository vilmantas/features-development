using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyManager : Manager
    {
        private Transform m_Character;

        private LevelManager m_LevelManager;

        public override void Initialize()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player").transform;

            m_LevelManager = GameObject.FindGameObjectWithTag(nameof(LevelManager)).GetComponent<LevelManager>();

            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_LevelManager.SpawnIntervalSeconds);

                var position = m_Character.position;

                var rand = Random.insideUnitCircle;

                var newPos = new Vector3(position.x + rand.x, position.y, position.z + rand.y);

                var dir = (newPos - position).normalized;

                var spawnPos = position + dir * Random.Range(20, 50);
                
                var newEnemy = Instantiate(GetRandomEnemy(), spawnPos, Quaternion.identity);

                newEnemy.TimeToLive = Random.Range(5f, 20f);
            }
        }

        private EnemyController GetRandomEnemy()
        {
            return m_LevelManager.LevelEnemies[Random.Range(0, m_LevelManager.LevelEnemies.Length)];
        }
    }
}