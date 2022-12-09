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
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            SkillImplementationRegistry.Register(nameof(MeteorStrikeSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            

            return new SkillActivationResult(true);
        }

        private static void OnReceive(SkillActivationContext obj)
        {
        }

        private static void OnRemove(SkillActivationContext obj)
        {
        }
    }
}