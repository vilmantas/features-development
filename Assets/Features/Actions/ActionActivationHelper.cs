using UnityEngine;

namespace Features.Actions
{
    public static class ActionActivationHelper
    {
        public static ActionActivation NoAction()
        {
            var NoActionImplementation = new ActionImplementation("NoAction", x => { });

            return new ActionActivation(NoActionImplementation, null);
        }
        
        public static ActionActivation GetActivation(ActionActivationPayload payload)
        {
            var result = NoAction();

            if (ImplementationRegistered(payload.Action, out var implementation))
            {
                result = new ActionActivation(implementation, payload); 
            }

            return result;
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