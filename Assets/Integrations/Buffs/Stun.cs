using Features.Buffs;
using UnityEngine;

namespace Integrations.Buffs
{
    public static class Stun
    {

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            BuffImplementation implementation = new(nameof(Stun), OnReceive, OnRemove);
            BuffImplementationRegistry.Register(implementation);
        }

        private static void OnReceive(BuffActivationPayload buffActivationPayload)
        {
            Debug.Log("Received STUN");
        }

        private static void OnRemove(BuffActivationPayload buffActivationPayload)
        {
            Debug.Log("STUN Over");
        }
    }
}