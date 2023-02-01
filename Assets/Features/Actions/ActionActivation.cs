namespace Features.Actions
{
    public class ActionActivation
    {
        private readonly ActionImplementation Implementation;

        public readonly ActionActivationPayload Payload;

        public bool PreventDefault = false;

        public ActionActivation(ActionImplementation implementation, ActionActivationPayload payload)
        {
            Implementation = implementation;
            Payload = payload;
        }

        public ActionActivationResult Activate()
        {
            var actionPayload = Implementation.PayloadFactory?.Invoke(Payload) ?? Payload;

            if (Implementation.ActivationWithResultAction != null)
            {
                return Implementation.ActivationWithResultAction.Invoke(actionPayload);
            }
            
            Implementation.ActivationAction.Invoke(actionPayload);

            return ActionActivationResult.NoResultActivation;
        }
    }
}