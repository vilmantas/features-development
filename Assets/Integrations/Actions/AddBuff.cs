using System;
using Features.Actions;
using Features.Buffs;
using Features.Combat;
using Features.Movement;
using UnityEngine;

namespace Integrations.Actions
{
    public static class AddBuff
    {

        public static AddBuffActionPayload MakePayload(GameObject source, GameObject target,
            BuffBase buff, float overrideDuration)
        {
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(AddBuff)), source, target);

            var addOptions = new BuffAddOptions(buff, source, 1) {OverrideDuration = overrideDuration};
            
            return new AddBuffActionPayload(basePayload, addOptions);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(AddBuff), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name,
                implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not AddBuffActionPayload addBuffActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to addBuff action");
            }

            var buffController = payload.Target.GetComponentInChildren<BuffController>();

            if (!buffController) return;
            
            buffController.AttemptAdd(addBuffActionPayload.Data);
        }

        public class AddBuffActionPayload : ActionActivationPayload
        {
            public readonly BuffAddOptions Data;

            public AddBuffActionPayload(ActionActivationPayload original, BuffAddOptions buff) :
                base(original.Action,
                    original.Source, original.Target)
            {
                Data = buff;
            }
        }
    }
}