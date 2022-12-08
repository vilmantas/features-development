using System;
using Features.Actions;
using Features.Cooldowns;
using Features.Skills;
using UnityEngine;

namespace Integrations.Skills.Actions
{
    public static class ActivateSkill
    {
        public static ActionActivationPayload MakePayload(GameObject source, SkillMetadata skill)
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
            if (payload is not ActivateSkillActionPayload skillActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to activateSkill action");
            }

            var controller = payload.Target.GetComponentInChildren<SkillsController>();
            
            if (!controller) return;
            
            var ctx = new SkillActivationContext(skillActionPayload.Skill,
                skillActionPayload.Target);
            
            controller.ActivateSkill(ctx);
        }

        public class ActivateSkillActionPayload : ActionActivationPayload
        {
            public readonly SkillMetadata Skill;

            public ActivateSkillActionPayload(ActionActivationPayload original,
                SkillMetadata skill) : base(original.Action,
                original.Source, original.Target, original.Data)
            {
                Skill = skill;
            }
        }
    }
}