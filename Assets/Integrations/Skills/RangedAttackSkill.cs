using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class RangedAttackSkill
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SkillImplementation implementation = new(OnActivation);
            SkillImplementationRegistry.Register(nameof(RangedAttackSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            Debug.Log("Doing ranged attack");

            return new SkillActivationResult(true);
        }
    }
}