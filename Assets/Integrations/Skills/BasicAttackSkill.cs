using Features.Actions;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class BasicAttackSkill
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            SkillImplementationRegistry.Register(nameof(BasicAttackSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            var payload =
                new ActionActivationPayload(new ActionBase(nameof(Strike)), context.Source);

            var a = context.Source.GetComponentInChildren<ActionsController>();

            a.DoAction(payload);

            return new SkillActivationResult(true);
        }
    }
}