using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Feature.Health.Events
{
    [Serializable]
    public class DamageReceived : UnityEvent<HealthChangeResult> {}
    
    [Serializable]
    public class HealingReceived : UnityEvent<HealthChangeResult> {}

    public class HealthChangeResult
    {
        public readonly int Before;

        public readonly int After;

        public readonly int OriginalChange;
        
        public int ActualChange => After - Before;

        public HealthChangeResult(int before, int after, int originalChange)
        {
            Before = before;
            After = after;
            OriginalChange = originalChange;
        }
    }
}
