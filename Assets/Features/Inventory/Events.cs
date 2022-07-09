using System;
using Inventory.Abstract.Internal;
using UnityEngine.Events;
using Utilities.ItemsContainer;

namespace Features.Inventory.Events
{
    [Serializable]
    public class ChangeRequestHandledEvent : UnityEvent<IChangeRequestResult>
    {
    }

    [Serializable]
    public class ActionSelectedEvent : UnityEvent<StorageData, string>
    {
    }

    [Serializable]
    public class ContextRequestEvent : UnityEvent<StorageData>
    {
    }
}