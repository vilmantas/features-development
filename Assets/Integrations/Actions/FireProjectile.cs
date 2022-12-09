using System;
using System.Collections.Generic;
using System.ComponentModel;
using Features.Actions;
using Features.Combat;
using Features.Health;
using UnityEngine;

namespace Integrations.Actions
{
    public static class FireProjectile
    {
        public static FireProjectileActionPayload MakePayload(object damageSource, GameObject parent, GameObject target,
            string ammoType, Vector3 location, Vector3 direction, ProjectileController projectile, Action<ProjectileCollisionData> callback)
        {
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(FireProjectile)), parent, target);

            return new FireProjectileActionPayload(basePayload, ammoType, location, direction,
                damageSource, true, projectile, callback);
        }
        
        public static FireProjectileActionPayload MakeNoAmmoPayload(object damageSource, GameObject parent, GameObject target, Vector3 location, Vector3 direction, ProjectileController projectileController, Action<ProjectileCollisionData> callback)
        {
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(FireProjectile)), parent, target);

            return new FireProjectileActionPayload(basePayload, "", location, direction,
                damageSource, false, projectileController, callback);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(FireProjectile), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);

        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not FireProjectileActionPayload firePayload) return;

            var combatController = firePayload.Source.GetComponentInChildren<CombatController>();

            if (!combatController) return;

            ProjectileController projectile = firePayload.Projectile;
            
            if (firePayload.RequiresAmmo)
            {
                if (!combatController.AmmoData.TryGetValue(firePayload.AmmoType, out projectile))
                {
                    Debug.Log("Ammo not provided!");

                    return;
                }
            }

            if (firePayload.Target == null)
            {
                combatController.FireProjectile(projectile, firePayload.Location,
                    firePayload.Direction, firePayload.DamageSource, firePayload.Callback);
            }
            else
            {
                combatController.FireHomingProjectile(projectile, firePayload.Location,
                    firePayload.Target, firePayload.DamageSource, firePayload.Callback);
            }
        }
    }

    public class FireProjectileActionPayload : ActionActivationPayload
    {
        public readonly Action<ProjectileCollisionData> Callback;
        
        public readonly Vector3 Direction;
        
        public readonly Vector3 Location;

        public readonly ProjectileController Projectile;

        public readonly string AmmoType;

        public readonly object DamageSource;

        public readonly bool RequiresAmmo;

        public FireProjectileActionPayload(ActionActivationPayload original, string ammoType,
            Vector3 location,
            Vector3 direction,
            object damageSource, bool requiresAmmo, ProjectileController projectile, Action<ProjectileCollisionData> callback) : base(original.Action,
            original.Source, original.Target, original.Data)

        {
            Callback = callback;
            Location = location;
            AmmoType = ammoType;
            Direction = direction;
            DamageSource = damageSource;
            RequiresAmmo = requiresAmmo;
            Projectile = projectile;
        }
    }
}