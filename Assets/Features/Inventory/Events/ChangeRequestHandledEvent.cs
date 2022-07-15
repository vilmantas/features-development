using System;
using Features.Inventory.Abstract.Internal;
using UnityEngine.Events;

namespace Features.Inventory.Events
{
    [Serializable]
    public class ChangeRequestHandledEvent : UnityEvent<IChangeRequestResult>
    {
    }
}