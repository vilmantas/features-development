using Features.Buffs;
using UnityEngine;

namespace Features.Character.Buffs
{
    public static class LifterImplementation
    {
        public static readonly BuffImplementation Implementation = new("Lifter", OnReceive, OnRemove, null, null);

        private static void OnReceive(BuffActivationPayload payload)
        {
            var liftAmount = Vector3.up * 5;

            payload.Target.transform.Translate(liftAmount);

            payload.Buff.State = new LifterState("Lifted", liftAmount);
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(-((LifterState) payload.Buff.State).LiftAmount);
        }

        private class LifterState : IBuffState
        {
            public LifterState(string description, Vector3 liftAmount)
            {
                Description = description;
                LiftAmount = liftAmount;
            }

            public Vector3 LiftAmount { get; }

            public string Description { get; }
        }
    }
}