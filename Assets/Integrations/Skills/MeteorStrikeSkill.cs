using System.Linq;
using Features.Actions;
using Features.Buffs;
using Features.Combat;
using Features.Conditions;
using Features.Skills;
using Integrations.Actions;
using Integrations.Buffs;
using Integrations.GameSystems;
using Integrations.StatusEffects;
using UnityEngine;
using UnityEngine.AI;

namespace Integrations.Skills
{
    public static class MeteorStrikeSkill
    {
        private static ProjectileController Projectile { get; set; }

        private static ParticleSystem Particles { get; set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            
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
            var particlePlayer = GameObject.Find("ROOT_SYSTEMS").GetComponentInChildren<ParticlePlayer>();

            var hitPoint = obj.Projectile.transform.position;
            
            particlePlayer.PlayParticles(Particles, hitPoint);

            var z = Physics.OverlapSphere(hitPoint, 10f, LayerMask.GetMask("PlayerHitbox"));

            foreach (var collider in z)
            {
                var colliderRoot = collider.transform.root;
                
                var actionsController =
                    colliderRoot.GetComponentInChildren<ActionsController>();

                var shovePayload = AddBuff.MakePayload(obj.ProjectileParent,
                    colliderRoot.gameObject, new BuffMetadata(nameof(Shove), 1f), 1f);
                
                var rb = colliderRoot.GetComponentInChildren<Rigidbody>();

                actionsController.DoPassiveAction(shovePayload);

                rb.AddExplosionForce(500f, hitPoint, 10f);
            }
            
            obj.SetProjectileConsumed();
        }
    }
}