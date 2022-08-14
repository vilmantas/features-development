using UnityEngine;

namespace Features.Actions
{
    public static class ActionActivationHelper
    {
        public static ActionActivation GetActivation(ActionActivationPayload payload)
        {
            if (!ImplementationRegistered(payload.Action, out var implementation)) return null;

            return new ActionActivation(implementation, payload);
        }

        private static bool ImplementationRegistered(ActionBase action, out ActionImplementation implementation)
        {
            if (!ActionImplementationRegistry.Implementations.TryGetValue(action.Name,
                    out implementation))
            {
                Debug.Log($"Implementation missing for action: {action.Name}");
            }

            return ActionImplementationRegistry.Implementations.TryGetValue(action.Name,
                out implementation);
        }
    }
}