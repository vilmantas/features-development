using Features.Equipment;
using UnityEngine;

namespace DebugScripts
{
    public class DebugEquipmentData : IEquipmentItemInstance
    {
        public DebugEquipmentData(DebugItemInstance parent, string mainSlot, string secondarySlot)
        {
            Parent = parent;
            this.mainSlot = mainSlot;
            this.secondarySlot = secondarySlot;
            Sprite = parent.Metadata.Sprite;
            ModelPrefab = parent.Metadata.ModelPrefab;
        }

        public GameObject ModelPrefab { get; }
        public Sprite Sprite { get; }
        public DebugItemInstance Parent { get; }
        public string mainSlot { get; }
        public string secondarySlot { get; }

        public bool IsStackable => Parent.StorageData.StackableData.Max > 1;
        public IEquipmentItemMetadata Metadata => Parent.Metadata;

        public string GetAmmoText => IsStackable ? CurrentAmount.ToString() : string.Empty;

        public int CurrentAmount => Parent.StorageData.StackableData.Current;

        public bool Combine(IEquipmentItemInstance other)
        {
            if (other is not DebugEquipmentData otherItem) return false;

            if (!otherItem.Parent.Equals(Parent)) return false;

            if (!IsStackable) return false;

            var amountToAdd = otherItem.CurrentAmount;

            Parent.StorageData.StackableData.Receive(otherItem.CurrentAmount,
                out int leftovers);

            otherItem.Parent.StorageData.StackableData.Reduce(otherItem.CurrentAmount - leftovers, out _);

            return true;
        }
    }
}