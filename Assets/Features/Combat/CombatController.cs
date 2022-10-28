using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Features.Combat
{
    public class CombatController : MonoBehaviour
    {
        private Dictionary<string, ProjectileController> m_AmmoData;

        public Action<ProjectileCollisionData> OnProjectileCollided;
        
        public IReadOnlyDictionary<string, ProjectileController> AmmoData => m_AmmoData;

        private void Awake()
        {
            m_AmmoData = new Dictionary<string, ProjectileController>();
        }

        public void FireProjectile(ProjectileController projectilePrefab, Vector3 location,
            Vector3 direction, object source)
        {
            var projectile = Instantiate(projectilePrefab, location, Random.rotation);

            projectile.OnProjectileCollided += data => OnProjectileCollided?.Invoke(data);
            
            projectile.Initialize(transform.root.gameObject, source, direction);
        }

        public void SetAmmo(string ammoName, ProjectileController prefab)
        {
            if (m_AmmoData.ContainsKey(ammoName)) return;
            
            m_AmmoData[ammoName] = prefab;
        }

        public void RemoveAmmo(string ammoName)
        {
            m_AmmoData.Remove(ammoName);
        }
    }
}