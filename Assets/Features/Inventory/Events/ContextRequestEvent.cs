using System;
using UnityEngine.Events;
using Utilities.ItemsContainer;

namespace Features.Inventory.Events
{
    [Serializable]
    public class ContextRequestEvent : UnityEvent<StorageData>
    {
    }
}