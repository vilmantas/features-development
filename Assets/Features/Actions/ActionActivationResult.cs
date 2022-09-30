namespace Features.Actions
{
    public class ActionActivationResult
    {
        public static ActionActivationResult NoResultActivation = new ActionActivationResult(null);
        
        public readonly bool? IsSuccessful;

        public ActionActivationResult(bool? isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}