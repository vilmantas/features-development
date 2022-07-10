using System;

namespace Features.Buffs
{
    public class BuffImplementation
    {
        public BuffImplementation(string name,
            Action<BuffActivationPayload> onReceive,
            Action<BuffActivationPayload> onRemove,
            Action<BuffActivationPayload> onTick)
        {
            Name = name;
            OnReceive = onReceive;
            OnRemove = onRemove;
            OnTick = onTick;
        }

        public string Name { get; }

        public Action<BuffActivationPayload> OnReceive { get; }

        public Action<BuffActivationPayload> OnRemove { get; }

        public Action<BuffActivationPayload> OnTick { get; }
    }
}