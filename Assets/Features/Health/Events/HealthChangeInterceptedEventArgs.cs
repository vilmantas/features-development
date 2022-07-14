namespace Features.Health.Events
{
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