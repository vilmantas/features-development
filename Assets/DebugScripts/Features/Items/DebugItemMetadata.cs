using Features.Equipment;
using Features.Inventory.UI.Utilities;
using UnityEngine;

namespace DebugScripts
{
    public class DebugItemMetadata : IInventoryItemMetadata, IEquipmentItemMetadata
    {
        public DebugItemMetadata(string name, Sprite sprite, GameObject modelPrefab, string mainSlot,
            string secondarySlot, bool isStackable, int maxStack = 1)
        {
            Name = name;
            Sprite = sprite;
            ModelPrefab = modelPrefab;
            MainSlot = mainSlot;
            SecondarySlot = secondarySlot;
            IsStackable = isStackable;
            MaxStack = maxStack;
        }

        public GameObject ModelPrefab { get; }
        public string MainSlot { get; }
        public string SecondarySlot { get; }
        public bool IsStackable { get; }

        public string Name { get; }
        public Sprite Sprite { get; }
        public int MaxStack { get; }
    }
}