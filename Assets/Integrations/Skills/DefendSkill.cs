using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class DefendSkill
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            SkillImplementationRegistry.Register(nameof(DefendSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var defendAction = Defend.MakePayload(context.Source, true);

            var result = context.Source.GetComponentInChildren<ActionsController>()
                .DoAction(defendAction);

            return new SkillActivationResult(result.IsSuccessful.HasValue &&
                                             result.IsSuccessful.Value);
        }
    }
}