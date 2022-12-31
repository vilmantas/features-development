using System.Linq;
using Features.Actions;
using Features.Buffs;
using Features.Combat;
using Features.Skills;
using Integrations.Actions;
using Integrations.Buffs;
using Integrations.GameSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Integrations.Skills
{
    public static class MeteorStrikeSkill
    {
        private static ProjectileController Projectile { get; set; }

        private static ParticleSystem Particles { get; set; }
        
        private static GameObject Sphere { get; set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            
            SkillImplementationRegistry.Register(nameof(MeteorStrikeSkill), implementation);
            
            Projectile = Resources.Load<ProjectileController>("Prefabs/Meteor");
            
            Sphere = Resources.Load<GameObject>("Prefabs/DebugSphere");

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

            var position = obj.Projectile.transform.position;
            particlePlayer.PlayParticles(Particles, position);

            var z = Physics.OverlapSphere(position, 10f, LayerMask.GetMask("PlayerHitbox"));

            foreach (var collider in z)
            {
                var position1 = collider.transform.position;
                var dist = position1 - position;

                var dir = dist.normalized;
                
                var xx = particlePlayer.CreateInstanceOf(Sphere);

                xx.transform.position = position1 + (dir * 3f);

                var zoot = collider.transform.root;
                
                var actionsController =
                    zoot.GetComponentInChildren<ActionsController>();

                var rb = zoot.GetComponentInChildren<Rigidbody>();

                zoot.GetComponentInChildren<NavMeshAgent>().enabled = false;

                rb.isKinematic = false;
                
                rb.AddForce(dir * 20f);
                
                var stunPayload = AddBuff.MakePayload(obj.ProjectileParent,
                    zoot.gameObject, new BuffMetadata(nameof(Stun), 1f), 1f);

                actionsController.DoPassiveAction(stunPayload);
            }
            
            var zz = particlePlayer.CreateInstanceOf(Sphere);

            zz.transform.position = position;
            
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