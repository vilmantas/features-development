using System;
using Features.Actions;
using Features.Combat;
using Features.Cooldowns;
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

            var cooldowns = payload.Target.GetComponentInChildren<CooldownsController>();

            if (cooldowns.IsOnCooldown(activateSkillActionPayload.Skill))
            {
                Debug.Log(activateSkillActionPayload.Skill +" is on cooldown");
                return;
            }
            var ctx = new SkillActivationContext(activateSkillActionPayload.Skill,
                activateSkillActionPayload.Target);
            
            controller.ActivateSkill(ctx);
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