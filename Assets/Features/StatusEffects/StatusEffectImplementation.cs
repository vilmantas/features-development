using System;

namespace Features.Conditions
{
    public class StatusEffectImplementation
    {
        public string Name;
        
        public Action<StatusEffectPayload> OnStatusEffectApplied;

        public Action<StatusEffectPayload> OnStatusEffectRemoved;

        public StatusEffectImplementation(string name, Action<StatusEffectPayload> onStatusEffectApplied, Action<StatusEffectPayload> onStatusEffectRemoved)
        {
            Name = name;
            OnStatusEffectApplied = onStatusEffectApplied;
            OnStatusEffectRemoved = onStatusEffectRemoved;
        }

        public void Apply(StatusEffectPayload payload)
        {
            OnStatusEffectApplied.Invoke(payload);
        }

        public void Remove(StatusEffectPayload payload)
        {
            OnStatusEffectRemoved.Invoke(payload);
        }
    }
}