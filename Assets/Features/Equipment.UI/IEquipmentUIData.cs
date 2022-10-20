using System;
using UnityEngine;

namespace Features.Equipment.UI
{
    public interface IEquipmentUIData
    {
        Action<EquipmentContainerItem> OnPressed { get; set; }

        GameObject gameObject { get; }

        void SetData(EquipmentContainerItem slot);

        void Reset();
        
        void Unsubscribe();
    }
}