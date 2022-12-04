using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.Skills
{
    public static class BasicAttackSkill
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            SkillImplementation implementation = new(OnReceive, OnActivation, OnRemove);
            SkillImplementationRegistry.Register(nameof(BasicAttackSkill), implementation);
        }
        
        private static SkillActivationResult OnActivation(SkillActivationContext context)
        {
            Debug.Log("Doing basic attack");

            var payload =
                new ActionActivationPayload(new ActionBase(nameof(Strike)), context.Source);

            var a = context.Source.GetComponentInChildren<ActionsController>();

            a.DoAction(payload);

            return new SkillActivationResult(true);
        }

        private static void OnReceive(SkillActivationContext obj)
        {
            Debug.Log(obj.Source.name + " Was given BasicAttack");
        }

        private static void OnRemove(SkillActivationContext obj)
        {
            Debug.Log(obj.Source.name + " Lost BasicAttack!");
        }
    }
}