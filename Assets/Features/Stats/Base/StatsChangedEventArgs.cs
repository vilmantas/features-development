namespace Stats
{
    public class StatsChangedEventArgs
    {
        public readonly StatGroup Difference;

        public readonly StatGroup New;
        public readonly StatGroup Previous;

        public StatsChangedEventArgs(StatGroup previous, StatGroup @new)
        {
            Previous = previous;
            New = @new;
            Difference = New.Difference(Previous);
        }
    }
}