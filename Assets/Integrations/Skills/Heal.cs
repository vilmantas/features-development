using Features.Actions;
using Features.CharacterModel;
using Features.ParticlePlayer;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class HealSkill
    {
        public static readonly int HEAL_DEFAULT_VALUE = 10;

        private static ParticleSystem m_HealParticles;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            SkillImplementationRegistry.Register(nameof(HealSkill), implementation);

            var particles = Resources.Load<ParticleSystem>("Particles/Heal");

            if (!particles) return;

            m_HealParticles = particles;
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var healVal = HEAL_DEFAULT_VALUE;
            
            if (context.Metadata.Extras.TryGetValue("power", out var data))
            {
                healVal = (int)data.NumericValue;
            }
            
            var healAction = Heal.MakePayload(context.Source, context.TargetObject, healVal);

            var result = context.Source.GetComponentInChildren<ActionsController>()
                .DoAction(healAction);

            var success = result == ActionActivationResult.NoResultActivation ||
                          result.IsSuccessful.HasValue &&
                          result.IsSuccessful.Value;

            if (!success) return new SkillActivationResult(false);
            
            var x = context.TargetObject.GetComponentInChildren<CharacterModelController>();
                
            ParticlePlayerController.Instance.PlayParticles(m_HealParticles,
                x.HeadLocation.position);

            return new SkillActivationResult(true);
        }
    }
}