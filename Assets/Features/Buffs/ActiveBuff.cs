using System;
using Utilities;

namespace Features.Buffs
{
    public class ActiveBuff
    {
        public const float BUFF_INTERVAL_MIN = 0.1f;

        internal readonly ResourceContainer Counter;

        public readonly BuffBase Metadata;
        
        private Action<ActiveBuff> m_OnStackAdded;

        private Action<ActiveBuff> m_OnStackRemoved;


        internal ActiveBuff(BuffBase metadata)
        {
            Metadata = metadata;
            Counter = new ResourceContainer(metadata.MaxStack);

            TimeBeforeNextTick = metadata.TickImmediateExecution ? 0f : metadata.TickInterval;

            ResetDuration();
        }

        public float DurationLeft { get; private set; }

        public float TimeBeforeNextTick { get; private set; }

        public bool IsDepleted => Counter.IsEmpty;

        public int Stacks => Counter.Current;

        internal ActiveBuff RegisterCallbacks(Action<ActiveBuff> onStackRemoved,
            Action<ActiveBuff> onStackAdded)
        {
            m_OnStackAdded = onStackAdded;
            m_OnStackRemoved = onStackRemoved;

            return this;
        }

        public void Tick(float delta)
        {
            while (delta > BUFF_INTERVAL_MIN)
            {
                TickNormalized(BUFF_INTERVAL_MIN);

                delta -= BUFF_INTERVAL_MIN;
            }

            TickNormalized(delta);
        }

        private void TickNormalized(float delta)
        {
            if (IsDepleted) return;

            DurationLeft -= delta;

            TickInterval(delta);

            if (DurationLeft > 0) return;

            ConsumeStack(DurationLeft);
        }

        private void TickInterval(float delta)
        {
            if (!Metadata.TickingEnabled) return;

            TimeBeforeNextTick -= delta;

            if (TimeBeforeNextTick > 0) return;

            OnIntervalTick(TimeBeforeNextTick);
        }

        public void OnIntervalTick(float offset = 0)
        {
            Metadata.OnTick();

            TimeBeforeNextTick = Metadata.TickInterval - Math.Abs(offset);
        }

        internal void ConsumeStack(float offset = 0)
        {
            RemoveStacks(1);

            m_OnStackRemoved?.Invoke(this);

            ResetDuration(Math.Abs(offset));
        }

        internal void AddStacks(int stacks)
        {
            Counter.Receive(stacks, out int leftovers);

            m_OnStackAdded?.Invoke(this);

            if (leftovers > 0)
            {
                ResetDuration();
            }
        }

        internal void RemoveStacks(int stacks) => Counter.Reduce(stacks);

        internal void ResetDuration(float offset = 0f) => DurationLeft = Metadata.Duration - offset;
    }
}