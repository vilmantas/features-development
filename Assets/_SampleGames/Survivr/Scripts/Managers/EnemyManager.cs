using System;
using System.Collections;
using _SampleGames.Survivr.Scripts.TagComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyManager : Manager
    {
        private Transform m_PlayerTransform;
        
        private PlayerManager m_PlayerManager;

        private LevelSceneManager m_LevelSceneManager;

        public override void Initialize()
        {
            m_PlayerManager = FindObjectOfType<PlayerManager>();

            m_PlayerTransform = m_PlayerManager.Player.transform;

            m_LevelSceneManager = GameObject.FindGameObjectWithTag(nameof(LevelSceneManager)).GetComponent<LevelSceneManager>();

            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_LevelSceneManager.SpawnIntervalSeconds);

                var spawnPos = GetRandomPositionAroundPlayerInRange(50, 70);

                var enemy = Instantiate(GetRandomEnemy(), spawnPos, Quaternion.identity);
             
                enemy.Initialize(25, m_PlayerManager.Player);

                AddDamageOverTime(enemy);
            }
        }

        private static void AddDamageOverTime(EnemyController enemy)
        {
            var systemsContainer = enemy.GetComponentInChildren<SystemsContainer>();

            var damageOverTimeParent = new GameObject("damage_over_time");

            damageOverTimeParent.transform.parent = systemsContainer.transform;

            AddDamageOverTimeComponent(damageOverTimeParent);
        }

        private static void AddDamageOverTimeComponent(GameObject damageOverTimeParent)
        {
            var dmgComp = damageOverTimeParent.AddComponent<DamageOverTime>();

            dmgComp.Damage = 1;

            dmgComp.Interval = Random.Range(0.5f, 1f);

            dmgComp.Initialize();
        }

        private Vector3 GetRandomPositionAroundPlayerInRange(int min, int max)
        {
            var position = m_PlayerTransform.position;

            var rand = Random.insideUnitCircle;

            var newPos = new Vector3(position.x + rand.x, position.y, position.z + rand.y);

            var dir = (newPos - position).normalized;

            var spawnPos = position + dir * Random.Range(min, max);
            return spawnPos;
        }

        private EnemyController GetRandomEnemy()
        {
            return m_LevelSceneManager.LevelEnemies[Random.Range(0, m_LevelSceneManager.LevelEnemies.Length)];
        }
    }
}