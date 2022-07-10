using Features.Equipment.Events;
using UnityEngine;

namespace Features.Equipment
{
    public interface IEquipmentUIData
    {
        EquipmentButtonPressEvent OnPressed { get; }

        GameObject gameObject { get; }

        void SetData(EquipmentContainerItem slot);
    }
}