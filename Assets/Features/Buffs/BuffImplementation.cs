using System;
using Features.Buffs;

namespace Features.Buffs
{
    public class BuffImplementation<T>
    {
        public BuffImplementation(Action<T> onReceive, string name, Action<T> onRemove, Action<T> onTick)
        {
            OnReceive = onReceive;
            Name = name;
            OnRemove = onRemove;
            OnTick = onTick;
        }

        public string Name { get; }

        public Action<T> OnReceive { get; }
        
        public Action<T> OnRemove { get; }
        
        public Action<T> OnTick { get; }
    }
}