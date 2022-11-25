using System;
using System.Collections.Generic;
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
            BuffMetadata buff, float overrideDuration, bool isPassive = false)
        {
            var dataBag = new Dictionary<string, object>()
            {
                {"passive", isPassive}
            };
            
            var basePayload =
                new ActionActivationPayload(new ActionBase(nameof(AddBuff)), source, target, dataBag);

            var addOptions = new BuffAddOptions(buff, source, 1) {OverrideDuration = overrideDuration};
            
            return new AddBuffActionPayload(basePayload, addOptions);
        }
        
        public static AddBuffActionPayload MakePayloadPassive(GameObject source, GameObject target,
            BuffMetadata buff, float overrideDuration)
        {
            return MakePayload(source, target, buff, overrideDuration, true);
        }
        
        public static AddBuffActionPayload MakePayloadPassive(GameObject source, GameObject target,
            BuffMetadata buff)
        {
            return MakePayload(source, target, buff, -1f, true);
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
                    original.Source, original.Target, original.Data)
            {
                Data = buff;
            }
        }
    }
}