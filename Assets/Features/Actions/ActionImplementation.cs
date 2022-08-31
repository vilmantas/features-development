using System;

namespace Features.Actions
{
    public sealed class ActionImplementation
    {
        public readonly Action<ActionActivationPayload> ActivationAction;
        public readonly string Name;
        public readonly Func<ActionActivationPayload, ActionActivationPayload> PayloadFactory;

        public ActionImplementation(string name,
            Action<ActionActivationPayload> activationAction,
            Func<ActionActivationPayload, ActionActivationPayload> payloadFactory)
        {
            Name = name;
            ActivationAction = activationAction;
            PayloadFactory = payloadFactory;
        }
        
        public ActionImplementation(string name,
            Action<ActionActivationPayload> activationAction)
        {
            Name = name;
            ActivationAction = activationAction;
        }
    }
}