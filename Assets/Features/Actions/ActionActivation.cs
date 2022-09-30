namespace Features.Actions
{
    public class ActionActivation
    {
        private readonly ActionImplementation Action;

        public readonly ActionActivationPayload Payload;

        public ActionActivation(ActionImplementation action, ActionActivationPayload payload)
        {
            Action = action;
            Payload = payload;
        }

        public ActionActivationResult Activate()
        {
            var actionPayload = Action.PayloadFactory?.Invoke(Payload) ?? Payload;

            if (Action.ActivationWithResultAction != null)
            {
                return Action.ActivationWithResultAction.Invoke(actionPayload);
            }
            
            Action.ActivationAction.Invoke(actionPayload);

            return ActionActivationResult.NoResultActivation;
        }
    }
}