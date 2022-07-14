namespace Features.Health.Events
{
    public class HealthChangeEventArgs
    {
        public readonly HealthController Source;

        public readonly int After;
        public readonly int Before;

        public readonly int OriginalChange;

        public HealthChangeEventArgs(HealthController source, int before, int after, int originalChange)
        {
            Before = before;
            After = after;
            OriginalChange = originalChange;
            Source = source;
        }

        public int ActualChange => After - Before;
    }
}