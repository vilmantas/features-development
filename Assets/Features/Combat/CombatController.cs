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

        public Action<CombatActionPayload> OnBeforeStrike;

        public Action<BlockActionPayload> OnBeforeBlock;
        
        public Action<CombatActionPayload> OnStrike;
        
        public Action<Guid> OnStrikeInterrupted;

        public Action<Guid> OnStrikeCancelled;
        
        public Action<bool> OnBlockingStatusChanged;
        
        public bool IsBlocking { get; private set; }

        public IReadOnlyDictionary<string, ProjectileController> AmmoData => m_AmmoData;

        private void Awake()
        {
            m_AmmoData = new Dictionary<string, ProjectileController>();
        }

        public void FireProjectile(ProjectileController projectilePrefab, Vector3 spawnLocation,
            Vector3 direction, object source, Action<ProjectileCollisionData> callback)
        {
            var projectile = Instantiate(projectilePrefab, spawnLocation, Random.rotation);

            projectile.OnProjectileCollided += callback;
            
            projectile.Initialize(transform.root.gameObject, source, direction, null);
        }
        
        public void FireHomingProjectile(ProjectileController projectilePrefab, Vector3 spawnLocation,
            GameObject target, object source, Action<ProjectileCollisionData> callback)
        {
            var projectile = Instantiate(projectilePrefab, spawnLocation, Random.rotation);

            projectile.OnProjectileCollided += callback;
            
            projectile.Initialize(transform.root.gameObject, source, Vector3.zero, target);
        }

        public bool Strike(Guid id)
        {
            var payload = new CombatActionPayload(id);
            
            OnBeforeStrike?.Invoke(payload);

            if (payload.PreventDefault) return false;
            
            OnStrike?.Invoke(payload);

            return true;
        }

        public void SetBlocking(bool status)
        {
            if (IsBlocking == status) return;

            var payload = new BlockActionPayload();
            
            OnBeforeBlock?.Invoke(payload);

            if (payload.PreventDefault) return;

            IsBlocking = status;
            
            OnBlockingStatusChanged?.Invoke(status);
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

    public class CombatActionPayload
    {
        public readonly Guid Id;
        
        public bool PreventDefault;

        public CombatActionPayload(Guid id)
        {
            Id = id;
        }
    }

    public class BlockActionPayload
    {
        public bool PreventDefault;
    }
}