using UnityEngine;

namespace Equipment
{
    public interface IEquipmentItem
    {
        GameObject ModelPrefab { get; }

        string mainSlot { get; }

        string secondarySlot { get; }

        Sprite Sprite { get; }

        public string GetAmmoText { get; }

        bool IsStackable { get; }

        int CurrentAmount { get; }

        public bool Combine(IEquipmentItem other);
    }

    public interface IEquipmentItem<out T> : IEquipmentItem
    {
        T Parent { get; }
    }
}