using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class MeteorStrikeSkill
    {
        public static ProjectileController Projectile { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            SkillImplementationRegistry.Register(nameof(MeteorStrikeSkill), implementation);
            
            Projectile = Resources.Load<ProjectileController>("Prefabs/Meteor");
        }

        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var comb = context.Source.GetComponentInChildren<ActionsController>();

            var spawnPoint = context.TargetLocation;

            spawnPoint.y += 10f;

            var projectilePayload = FireProjectile.MakePayload(
                context.Metadata, 
                context.Source,
                spawnPoint, 
                Callback,
                context.TargetObject);
            
            projectilePayload.Direction = Vector3.down;

            projectilePayload.Projectile = Projectile;

            comb.DoAction(projectilePayload);
                
            return new SkillActivationResult(true);
        }

        private static void Callback(ProjectileCollisionData obj)
        {
            obj.SetProjectileConsumed();
            Debug.Log("BOOM");
        }

        private static void OnReceive(SkillActivationContext obj)
        {
        }

        private static void OnRemove(SkillActivationContext obj)
        {
        }
    }
}