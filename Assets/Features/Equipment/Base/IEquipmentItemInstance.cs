using UnityEngine;

namespace Features.Equipment
{
    public interface IEquipmentItemMetadataBase
    {
        string MainSlot { get; }
        string SecondarySlot { get; }
    }

    public interface IEquipmentItemMetadata : IEquipmentItemMetadataBase
    {
        GameObject ModelPrefab { get; }
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
}