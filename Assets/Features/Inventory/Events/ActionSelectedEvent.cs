using System;
using UnityEngine.Events;
using Utilities.ItemsContainer;

namespace Features.Inventory.Events
{
    [Serializable]
    public class ActionSelectedEvent : UnityEvent<StorageData, string>
    {
    }
}