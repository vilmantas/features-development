using System;
using Utilities;

namespace Features.Buffs
{
    public class ActiveBuff
    {
        public readonly BuffBase Metadata;

        public readonly ResourceContainer Stacks;

        public float DurationLeft;

        public ActiveBuff(BuffBase metadata)
        {
            Metadata = metadata;
            Stacks = new ResourceContainer(metadata.MaxStack, 1);
        }

        public void ResetDuration()
        {
            DurationLeft = Metadata.Duration;
        }
    }

    public class BuffBase
    {
        public readonly float Duration;

        public readonly int MaxStack;
        public readonly string Name;

        public readonly Action<float> OnTick;

        public BuffBase(string name, float duration, Action<float> tick, int maxStack = 1)
        {
            Name = name;
            Duration = duration;
            OnTick = tick;
            MaxStack = maxStack;
        }
    }
}