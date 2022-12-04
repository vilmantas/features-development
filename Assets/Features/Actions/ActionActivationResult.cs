namespace Features.Actions
{
    public class ActionActivationResult
    {
        public static NoResultActivationResult NoResultActivation = new NoResultActivationResult();

        public static PreventedActivationResult PreventedActivation = new PreventedActivationResult();
        
        public static MissingImplementationResult MissingImplementation = new MissingImplementationResult();
        
        public readonly bool? IsSuccessful;

        public bool IsFailed => IsSuccessful.HasValue && !IsSuccessful.Value;

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
    
    public class MissingImplementationResult : ActionActivationResult
    {
        public MissingImplementationResult() : base(null)
        {
        }
    }
}