using System;
using UnityEngine;

namespace Features.Buffs
{
    public static class BuffActivationHelper
    {
        public static void Activate(ActiveBuff buff, Func<BuffImplementation, Action<BuffActivationPayload>> action,
            GameObject target)
        {
            if (!ImplementationRegistered(buff, out var implementation)) return;

            var payload = new BuffActivationPayload(buff.Source, target, buff);

            action(implementation)?.Invoke(payload);
        }

        private static bool ImplementationRegistered(ActiveBuff buff, out BuffImplementation implementation)
        {
            if (!BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                    out implementation))
            {
                Debug.Log($"Implementation missing for buff: {buff.Metadata.Name}");
            }

            return BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                out implementation);
        }
    }
}