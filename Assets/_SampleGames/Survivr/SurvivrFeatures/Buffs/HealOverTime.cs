using Features.Buffs;
using Features.Health;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Buffs
{
    public static class HealOverTime
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            BuffImplementation implementation = new(nameof(HealOverTime), OnReceive, OnRemove, OnTick, OnDurationReset);
            BuffImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnReceive(BuffActivationPayload payload)
        {
            payload.Buff.State = new HealOverTimeState(description: "");
        }

        private static void OnRemove(BuffActivationPayload payload)
        {
        }

        private static void OnTick(BuffActivationPayload payload)
        {
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (health)
            {
                health.Heal(5);
            }
        }

        private static void OnDurationReset(BuffActivationPayload payload)
        {
        }

        private class HealOverTimeState : IBuffState
        {
            public HealOverTimeState(string description)
            {
                Description = description;
            }

            public string Description { get; }
        }
    }
}