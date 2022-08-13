using System;
using UnityEngine;
using Utilities;

namespace Features.Buffs
{
    public class ActiveBuff
    {
        public const float BUFF_INTERVAL_MIN = 0.1f;
        internal readonly ResourceContainer Counter;
        public readonly BuffBase Metadata;

        private Action<ActiveBuff> m_OnDurationReset;
        private Action<ActiveBuff> m_OnStackAdded;
        private Action<ActiveBuff> m_OnStackRemoved;
        private Action<ActiveBuff> m_OnTickOccurred;

        public IBuffState State;

        internal ActiveBuff(BuffBase metadata, GameObject source) : this(metadata, source, -1f)
        {
        }

        internal ActiveBuff(BuffBase metadata, GameObject source, float duration)
        {
            Source = source;
            OverrideDuration = duration;
            Metadata = metadata;

            Counter = new ResourceContainer(metadata.MaxStack);

            TimeBeforeNextTick = metadata.TickImmediateExecution ? 0f : metadata.TickInterval;

            ResetDuration();
        }

        public float OverrideDuration { get; set; }

        public float Duration =>
            OverrideDuration > 0 ? OverrideDuration : Metadata.Duration;

        public GameObject Source { get; }

        public float DurationLeft { get; private set; }

        public float TimeBeforeNextTick { get; private set; }

        public bool IsDepleted => Counter.IsEmpty;

        public int Stacks => Counter.Current;

        public int MaxStacks => Counter.Max;

        internal void RegisterCallbacks(
            Action<ActiveBuff> onStackRemoved,
            Action<ActiveBuff> onStackAdded,
            Action<ActiveBuff> onTickOccured,
            Action<ActiveBuff> onDurationReset)
        {
            m_OnStackAdded = onStackAdded;
            m_OnStackRemoved = onStackRemoved;
            m_OnTickOccurred = onTickOccured;
            m_OnDurationReset = onDurationReset;
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
            m_OnTickOccurred?.Invoke(this);

            TimeBeforeNextTick = Metadata.TickInterval - Math.Abs(offset);
        }

        internal void ConsumeStack(float offset = 0)
        {
            RemoveStacks(1);

            ResetDuration(Math.Abs(offset));
        }

        internal void AddStacks(int stacks)
        {
            Counter.Receive(stacks, out int leftovers);

            if (leftovers != stacks)
            {
                m_OnStackAdded?.Invoke(this);
            }

            if (leftovers > 0)
            {
                ResetDuration();
            }
        }

        internal void RemoveStacks(int stacks)
        {
            Counter.Reduce(stacks);

            m_OnStackRemoved?.Invoke(this);
        }

        internal void ResetDuration(float offset = 0f)
        {
            DurationLeft = Duration - offset;

            m_OnDurationReset?.Invoke(this);
        }
    }
}