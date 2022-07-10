using System;
using UnityEngine.Events;

namespace Features.Health.Events
{
    [Serializable]
    public class DamageReceived : UnityEvent<HealthChangeResult>
    {
    }

    [Serializable]
    public class HealingReceived : UnityEvent<HealthChangeResult>
    {
    }

    public class HealthChangeResult
    {
        public readonly int After;
        public readonly int Before;

        public readonly int OriginalChange;

        public HealthChangeResult(int before, int after, int originalChange)
        {
            Before = before;
            After = after;
            OriginalChange = originalChange;
        }

        public int ActualChange => After - Before;
    }
}