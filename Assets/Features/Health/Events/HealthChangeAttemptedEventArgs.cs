namespace Features.Health.Events
{
    public class HealthChangeAttemptedEventArgs
    {
        public readonly int Amount;
        public readonly HealthController Target;

        public HealthChangeAttemptedEventArgs(HealthController target, int amount)
        {
            Target = target;
            Amount = amount;
        }
    }

    public class HealthChangeInterceptedEventArgs
    {
        public readonly HealthChangeAttemptedEventArgs Original;

        public readonly int NewAmount;

        public HealthChangeInterceptedEventArgs(HealthChangeAttemptedEventArgs originalEvent, int newAmount)
        {
            Original = originalEvent;
            NewAmount = newAmount;
        }
    }
}