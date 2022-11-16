using System;

namespace Features.Buffs
{
    public sealed class BuffImplementation
    {
        public readonly string Name;

        public readonly Action<BuffActivationPayload> OnDurationReset;

        public readonly Action<BuffActivationPayload> OnReceive;

        public readonly Action<BuffActivationPayload> OnRemove;

        public readonly Action<BuffActivationPayload> OnTick;

        public BuffImplementation(string name,
            Action<BuffActivationPayload> onReceive,
            Action<BuffActivationPayload> onRemove,
            Action<BuffActivationPayload> onTick = null,
            Action<BuffActivationPayload> onDurationReset = null)
        {
            Name = name;
            OnReceive = onReceive;
            OnRemove = onRemove;
            OnTick = onTick;
            OnDurationReset = onDurationReset;
        }
    }
}