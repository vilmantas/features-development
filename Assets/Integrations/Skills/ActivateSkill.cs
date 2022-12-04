using System;
using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using UnityEngine;

namespace Integrations.Skills.Actions
{
    public static class ActivateSkill
    {
        public static ActionActivationPayload MakePayload(GameObject source, string skill)
        {
            var payload = new ActionActivationPayload(new ActionBase(nameof(ActivateSkill)),
                source);

            return new ActivateSkillActionPayload(payload, skill);
        }
        

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(ActivateSkill), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not ActivateSkillActionPayload activateSkillActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to activateSkill action");
            }

            var controller = payload.Target.GetComponentInChildren<SkillsController>();
            
            if (!controller) return;
            
            controller.ActivateSkill(activateSkillActionPayload.Skill);
        }

        public class ActivateSkillActionPayload : ActionActivationPayload
        {
            public readonly string Skill;

            public ActivateSkillActionPayload(ActionActivationPayload original,
                string skillName) : base(original.Action,
                original.Source, original.Target, original.Data)
            {
                Skill = skillName;
            }
        }
    }
}