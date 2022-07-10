using System;
using UnityEngine.Events;

namespace Features.Buffs.Events
{
    [Serializable]
    public class BuffTimerTickEvent : UnityEvent<float>
    {
    }
}