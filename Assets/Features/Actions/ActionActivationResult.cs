namespace Features.Actions
{
    public class ActionActivationResult
    {
        public static NoResultActivationResult NoResultActivation = new NoResultActivationResult();

        public static PreventedActivationResult PreventedActivation = new PreventedActivationResult();
        
        public readonly bool? IsSuccessful;

        public ActionActivationResult(bool? isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }

    public class NoResultActivationResult : ActionActivationResult
    {
        public NoResultActivationResult() : base(null)
        {
        }
    }
    
    public class PreventedActivationResult : ActionActivationResult
    {
        public PreventedActivationResult() : base(null)
        {
        }
    }
}