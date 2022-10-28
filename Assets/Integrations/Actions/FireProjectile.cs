using System;
using System.ComponentModel;
using Features.Actions;
using Features.Health;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class FireProjectile
    {
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
                    Source: CharacterController character
                } firePayload) return;

            Debug.Log("Firing projectile: " + firePayload.AmmoType);

            var projectile = firePayload.Projectile;

            if (!projectile)
            {
                projectile = LoadProjectile(firePayload.AmmoType, character.gameObject);
                
                if (!projectile)
                {
                    Debug.Log("Ammo not provided!");
                }
            }

            var obj = new GameObject("Lol");
            
            // obj.
        }

        private static GameObject LoadProjectile(string ammoType, GameObject source)
        {
            return new GameObject();
        }
    }

    public class FireProjectileActionPayload : ActionActivationPayload
    {
        public readonly Transform Location;

        public readonly GameObject Projectile;

        public readonly string AmmoType;

        public FireProjectileActionPayload(ActionActivationPayload original, int FireProjectileAmount) : base(original.Action,
            original.Source, original.Target)
        {
            FireProjectileAmount = FireProjectileAmount;
        }
    }
}