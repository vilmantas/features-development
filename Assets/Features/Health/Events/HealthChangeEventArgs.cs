namespace Features.Health.Events
{
    public class HealthChangeEventArgs
    {
        public readonly int After;
        public readonly int Before;

        public readonly int OriginalChange;

        public HealthChangeEventArgs(int before, int after, int originalChange)
        {
            Before = before;
            After = after;
            OriginalChange = originalChange;
        }

        public int ActualChange => After - Before;
    }
}