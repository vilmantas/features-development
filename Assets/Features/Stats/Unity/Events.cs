using System;
using UnityEngine.Events;

namespace Stats.Unity
{
    [Serializable]
    public class StatsChangedEvent : UnityEvent<StatsChangedEventArgs>
    {
    }
}