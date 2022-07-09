using System;
using UnityEngine.Events;

namespace Equipment.Unity
{
    [Serializable]
    public class ItemEquippedEvent : UnityEvent<EquipResult>
    {
    }

    [Serializable]
    public class ItemUnequipEvent : UnityEvent<EquipmentContainerItem>
    {
    }
}