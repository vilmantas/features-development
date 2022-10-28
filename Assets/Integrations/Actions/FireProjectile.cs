using System;
using System.ComponentModel;
using Features.Actions;
using Features.Combat;
using Features.Health;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class FireProjectile
    {
        public static FireProjectileActionPayload MakePayload(object source, GameObject target,
            string ammoType, Vector3 location, Vector3 direction)
        {
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(FireProjectile)), source, target);

            return new FireProjectileActionPayload(basePayload, ammoType, location, direction);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(FireProjectile), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not FireProjectileActionPayload
                {
                    Source: GameObject obj
                } firePayload) return;

            var combatController = obj.GetComponentInChildren<CombatController>();

            if (!combatController) return;

            Debug.Log("Firing projectile: " + firePayload.AmmoType);

            var projectile = LoadProjectile(firePayload.AmmoType, combatController);
            
            if (!projectile)
            {
                Debug.Log("Ammo not provided!");

                return;
            }

            combatController.FireProjectile(projectile, firePayload.Location,
                firePayload.Direction);
        }

        private static ProjectileController LoadProjectile(string ammoType, CombatController source)
        {
            var combatController = source.GetComponentInChildren<CombatController>();

            return combatController.AmmoData[ammoType];
        }
    }

    public class FireProjectileActionPayload : ActionActivationPayload
    {
        public readonly Vector3 Direction;
        
        public readonly Vector3 Location;

        public readonly string AmmoType;

        public FireProjectileActionPayload(ActionActivationPayload original, string ammoType,
            Vector3 location,
            Vector3 direction) : base(original.Action,
            original.Source, original.Target)

        {
            Location = location;
            AmmoType = ammoType;
            Direction = direction;
        }
    }
}