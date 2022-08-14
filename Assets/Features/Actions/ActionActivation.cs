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

        public void Activate()
        {
            Action.ActivationAction.Invoke(Payload);
        }
    }
}