using UnityEngine;

namespace Features.Actions
{
    public static class ActionActivationHelper
    {
        public static ActionActivation NoAction(ActionActivationPayload payload)
        {
            var NoActionImplementation = new ActionImplementation("NoAction", x => { });
            
            NoActionImplementation.ActivationWithResultAction = x => ActionActivationResult.MissingImplementation;

            return new ActionActivation(NoActionImplementation, payload);
        }
        
        public static ActionActivation GetActivation(ActionActivationPayload payload)
        {
            var result = NoAction(payload);

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