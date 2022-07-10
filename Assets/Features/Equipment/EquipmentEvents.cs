using System;
using UnityEngine.Events;

namespace Features.Equipment.Events
{
    [Serializable]
    public class ItemEquippedEvent : UnityEvent<EquipResult>
    {
    }

    [Serializable]
    public class ItemUnequipEvent : UnityEvent<EquipmentContainerItem>
    {
    }

    [Serializable]
    public class EquipmentButtonPressEvent : UnityEvent<EquipmentContainerItem>
    {
    }
}