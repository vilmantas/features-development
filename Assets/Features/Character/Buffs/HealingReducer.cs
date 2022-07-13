using Features.Buffs;
using Features.Health;
using Features.Health.Events;
using UnityEngine;

namespace Features.Character.Buffs
{
    public static class HealingReducer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffImplementation implementation = new("HealingReducer", OnReceive, OnRemove, null, null);
            BuffImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnReceive(BuffActivationPayload payload)
        {
            var ctrl = payload.Target.GetComponentInChildren<HealthController>();

            ctrl.OnHealingAttempted.AddListener(HealingLimiter);
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
            var ctrl = payload.Target.GetComponentInChildren<HealthController>();

            ctrl.OnHealingAttempted.RemoveListener(HealingLimiter);
        }

        private static void HealingLimiter(HealthChangeAttemptedEventArgs args)
        {
            args.Target.Receive(args.Amount - 1);
        }
    }
}