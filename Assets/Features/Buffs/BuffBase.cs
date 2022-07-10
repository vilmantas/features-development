using System;

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

        public Action OnTick { get; private set; }
        
        public float TickInterval { get; private set; }

        public bool TickImmediateExecution { get; private set; }

        public BuffBase WithInterval(float tickInterval, Action onTick, bool executeImmediately = false)
        {
            TickImmediateExecution = executeImmediately;
            TickInterval = tickInterval;
            OnTick = onTick;

            TickingEnabled = onTick != null && tickInterval >= ActiveBuff.BUFF_INTERVAL_MIN;

            return this;
        }
    }

    public class BuffBase<T, U> : BuffBase where T : class
    {
        public T Source { get; }
        
        public U Target { get; }
        
        public Action<T, U> OnReceive { get; }
        
        public Action<T, U> OnRemove { get; }

        public BuffBase(T source, U target, string name, float duration, Action<T, U> onReceive, Action<T, U> onRemove, int maxStack = 1) : base(name, duration, maxStack)
        {
            Target = target;
            Source = source;
            OnReceive = onReceive;
            OnRemove = onRemove;
        }
    }
}