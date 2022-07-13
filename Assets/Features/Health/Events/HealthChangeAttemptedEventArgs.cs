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
}