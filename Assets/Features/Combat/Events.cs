using System;
using Features.Combat;
using UnityEngine.Events;

namespace Feature.Combat.Events
{
    [Serializable]
    public class Defend : UnityEvent<Attack> {}
}