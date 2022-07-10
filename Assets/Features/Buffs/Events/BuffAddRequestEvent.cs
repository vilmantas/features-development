using System;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Buffs.Events
{
    [Serializable]
    public class BuffAddRequestEvent : UnityEvent<BuffBase, GameObject>
    {
    }
}