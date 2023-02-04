using Features.Actions;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class HealSkill
    {
        public static readonly int HEAL_DEFAULT_VALUE = 10;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            SkillImplementationRegistry.Register(nameof(HealSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var healVal = HEAL_DEFAULT_VALUE;
            
            if (context.Metadata.Extras.TryGetValue("power", out var data))
            {
                healVal = (int)data.NumericValue;
            }
            
            var defendAction = Heal.MakePayload(context.Source, context.TargetObject, healVal);

            var result = context.Source.GetComponentInChildren<ActionsController>()
                .DoAction(defendAction);

            return new SkillActivationResult(result == ActionActivationResult.NoResultActivation ||
                                             result.IsSuccessful.HasValue &&
                                             result.IsSuccessful.Value);
        }
    }
}