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

            if (IsImplementationRegistered(payload.Action, out var implementation))
            {
                result = new ActionActivation(implementation, payload); 
            }

            return result;
        }

        private static bool IsImplementationRegistered(ActionBase action, out ActionImplementation implementation)
        {
            var result = ActionImplementationRegistry.Implementations.TryGetValue(action.Name,
                out implementation);
            
            if (!result)
            {
                Debug.LogWarning($"Implementation missing for action: {action.Name}");
            }

            return result;
        }
    }
}