using System;

namespace Features.Actions
{
    public sealed class ActionImplementation
    {
        public readonly Action<ActionActivationPayload> ActivationAction;
        public readonly string Name;

        public ActionImplementation(string name, Action<ActionActivationPayload> activationAction)
        {
            Name = name;
            ActivationAction = activationAction;
        }
    }
}