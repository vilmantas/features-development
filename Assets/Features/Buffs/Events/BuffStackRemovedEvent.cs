using System;
using UnityEngine.Events;

namespace Features.Buffs.Events
{
    [Serializable]
    public class BuffStackRemovedEvent : UnityEvent<ActiveBuff>
    {
    }
}