using UnityEngine;

namespace Features.Equipment
{
    public interface IEquipmentItemMetadata
    {
        GameObject ModelPrefab { get; }
        string MainSlot { get; }
        string SecondarySlot { get; }
        Sprite Sprite { get; }
        bool IsStackable { get; }
    }

    public interface IEquipmentItemInstance
    {
        public IEquipmentItemMetadata Metadata { get; }
        public string GetAmmoText { get; }
        int CurrentAmount { get; }
        public bool Combine(IEquipmentItemInstance other);
    }

    public interface IEquipmentItemInstance<out T> : IEquipmentItemInstance
    {
        T Parent { get; }
    }
}