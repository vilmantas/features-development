using Features.Actions;
using Features.Combat;
using Features.Skills;
using Integrations.Actions;
using Integrations.GameSystems;
using UnityEngine;

namespace Integrations.Skills
{
    public static class MeteorStrikeSkill
    {
        public static ProjectileController Projectile { get; set; }

        public static ParticleSystem Particles { get; set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            
            SkillImplementationRegistry.Register(nameof(MeteorStrikeSkill), implementation);
            
            Projectile = Resources.Load<ProjectileController>("Prefabs/Meteor");

            Particles = Resources.Load<ParticleSystem>("Particles/Explosion");
        }

        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var comb = context.Source.GetComponentInChildren<ActionsController>();

            var spawnPoint = context.TargetLocation;

            if (context.Metadata.Target == SkillTarget.Character)
            {
                spawnPoint = context.TargetObject.transform.position;
            }
            
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
            var player = GameObject.Find("ROOT_SYSTEMS").GetComponentInChildren<ParticlePlayer>();

            player.PlayParticles(Particles, obj.Projectile.transform.position);
            
            obj.SetProjectileConsumed();
        }

        private static void OnReceive(SkillActivationContext obj)
        {
        }

        private static void OnRemove(SkillActivationContext obj)
        {
        }
    }
}