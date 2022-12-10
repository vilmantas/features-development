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
        public static FireProjectileActionPayload MakePayload(object damageSource,
            GameObject parent, Vector3 location,
            Action<ProjectileCollisionData> callback, GameObject target = null)
        {
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(FireProjectile)), parent, target);

            return new FireProjectileActionPayload(basePayload, location, damageSource, callback);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(FireProjectile), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
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

            combatController.FireProjectile(projectile, firePayload.Location,
                firePayload.Direction, firePayload.DamageSource, firePayload.Callback);
        }
    }

    public class FireProjectileActionPayload : ActionActivationPayload
    {
        public readonly Action<ProjectileCollisionData> Callback;

        public Vector3 Direction;

        public readonly Vector3 Location;

        public ProjectileController Projectile;

        public string AmmoType;

        public readonly object DamageSource;

        public bool RequiresAmmo => !string.IsNullOrEmpty(AmmoType);

        public FireProjectileActionPayload(
            ActionActivationPayload original,
            Vector3 location,
            object damageSource,
            Action<ProjectileCollisionData> callback) : base(original.Action,
            original.Source, original.Target, original.Data)

        {
            Callback = callback;
            Location = location;
            DamageSource = damageSource;
        }
    }
}