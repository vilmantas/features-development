using System;
using UnityEngine.Events;

namespace Features.Health.Events
{
    [Serializable]
    public class DamageReceivedEvent : UnityEvent<HealthChangeEventArgs>
    {
    }
}