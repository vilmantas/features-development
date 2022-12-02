using System;
using Features.Actions;
using Features.Combat;
using Features.Movement;
using Features.Skills;
using UnityEngine;

namespace Integrations.Actions
{
    public static class ActivateSkill
    {

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
            
            controller.ActivateSkill(activateSkillActionPayload.Data.Skill.InternalName);
        }

        public class ActivateSkillActionPayload : ActionActivationPayload
        {
            public readonly ActivateSkillActionData Data;

            public ActivateSkillActionPayload(ActionActivationPayload original,
                ActivateSkillActionData data) : base(original.Action,
                original.Source, original.Target, original.Data)
            {
                Data = data;
            }
        }

        public class ActivateSkillActionData
        {
            public readonly SkillMetadata Skill;

            public ActivateSkillActionData(SkillMetadata skill)
            {
                Skill = skill;
            }
        }
    }
}