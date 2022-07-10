namespace Features.Buffs
{
    public class BuffBase
    {
        public readonly float Duration;
        public readonly int MaxStack;
        public readonly string Name;

        public BuffBase(string name, float duration, int maxStack = 1)
        {
            Name = name;
            Duration = duration;
            MaxStack = maxStack;
        }

        public bool TickingEnabled { get; private set; }

        public float TickInterval { get; private set; }

        public bool TickImmediateExecution { get; private set; }

        public BuffBase WithInterval(float tickInterval, bool executeImmediately = false)
        {
            TickImmediateExecution = executeImmediately;
            TickInterval = tickInterval;

            TickingEnabled = tickInterval >= ActiveBuff.BUFF_INTERVAL_MIN;

            return this;
        }
    }
}