using System;

namespace Features.Conditions
{
    public class StatusEffectImplementation
    {
        public string Name;
        
        public Action OnStatusEffectApplied;

        public StatusEffectImplementation(string name, Action onStatusEffectApplied)
        {
            Name = name;
            OnStatusEffectApplied = onStatusEffectApplied;
        }
    }
}