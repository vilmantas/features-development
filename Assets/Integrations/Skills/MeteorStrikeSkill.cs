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
            Vector3 targetPosition;
            
            switch (context.Metadata.Target)
            {
                case SkillTarget.Character:
                    targetPosition = context.TargetObject.transform.position;
                    break;
                case SkillTarget.Self:
                    targetPosition = context.Source.transform.position;
                    break;
                case SkillTarget.Pointer:
                    targetPosition = context.TargetLocation;
                    break;
            }

            var comb = context.Source.GetComponentInChildren<ActionsController>();

            var p = context.Source.transform.position;

            p.y += 10f;

            var pp = FireProjectile.MakeNoAmmoPayload(context.Metadata, context.Source,
                context.TargetObject,
                p, Vector3.down, Projectile, Callback);

            comb.DoAction(pp);
                
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