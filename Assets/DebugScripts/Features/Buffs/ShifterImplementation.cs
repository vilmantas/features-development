using Features.Buffs;
using UnityEngine;

namespace Features.Character.Buffs
{
    public static class ShifterImplementation
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffImplementation implementation = new("Shifter", OnReceive, OnRemove, null, null);
            BuffImplementationRegistry.Register(implementation);
        }

        private static void OnReceive(BuffActivationPayload payload)
        {
            var shiftAmount = Vector3.forward * 5;

            payload.Target.transform.Translate(shiftAmount);

            payload.Buff.State = new ShifterState("Shifter", shiftAmount);
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(-((ShifterState) payload.Buff.State).ShiftAmount);
        }

        private class ShifterState : IBuffState
        {
            public ShifterState(string description, Vector3 shiftAmount)
            {
                Description = description;
                ShiftAmount = shiftAmount;
            }

            public Vector3 ShiftAmount { get; }

            public string Description { get; }
        }
    }
}