using System;
using Features.Equipment;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Utilities;
using UnityEngine;
using Utilities.ItemsContainer;
using Random = UnityEngine.Random;

namespace DebugScripts
{
    public class InventoryDebug : MonoBehaviour
    {
        public Transform UIContainer;

        public FakeItem_SO Arrows;

        public FakeItem_SO Axe;

        public FakeItem_SO Pickaxe;

        public BaseInventoryUIData baseInventoryUIPrefab;

        private InventoryController m_InventoryController;

        private void Start()
        {
            InventoryItemFactories.Register(typeof(FakeInventoryItemInstance), wtf);

            m_InventoryController = GetComponentInChildren<InventoryController>();

            m_InventoryController.OnChangeRequestHandled.AddListener(ChangeRequestHandled);

            m_InventoryController.WithUI(baseInventoryUIPrefab, UIContainer.transform);
        }

        private StorageData wtf(StorageData arg)
        {
            var itemInstance = arg.Parent as FakeInventoryItemInstance;

            return new FakeInventoryItemInstance(itemInstance.Metadata).StorageData;
        }

        private void ChangeRequestHandled(IChangeRequestResult arg0)
        {
        }

        public void GiveAxe()
        {
            var request = ChangeRequestFactory.Add(Axe.GetInstance.StorageData, 1);

            m_InventoryController.HandleRequest(request);
        }

        public void Give5Axes()
        {
            var request = ChangeRequestFactory.Add(Pickaxe.GetInstance.StorageData, 5);

            m_InventoryController.HandleRequest(request);
        }

        public void GiveRandomArrows()
        {
            var request = ChangeRequestFactory.Add(Arrows.GetInstance.StorageData, Random.Range(50, 100));

            m_InventoryController.HandleRequest(request);
        }
    }

    public class FakeInventoryItemInstance : IInventoryItemInstance, IEquatable<object>
    {
        public readonly EquipmentData EquipmentData;
        public readonly FakeInventoryItemMetadata Metadata;

        public readonly StorageData StorageData;
        private IInventoryItemInstance m_InventoryItemInstanceImplementation;

        public FakeInventoryItemInstance(FakeInventoryItemMetadata metadata)
        {
            Metadata = metadata;

            EquipmentData = new EquipmentData(this, "", "");

            StorageData = new StorageData(this, metadata.MaxStack);
        }

        IInventoryItemMetadata IInventoryItemInstance.Metadata => m_InventoryItemInstanceImplementation.Metadata;

        public override bool Equals(object other)
        {
            if (other is not FakeInventoryItemInstance b) return false;

            return Metadata.Name.Equals(b.Metadata.Name);
        }

        public static bool operator !=(FakeInventoryItemInstance a, FakeInventoryItemInstance b)
        {
            return a?.Metadata.Name != b?.Metadata.Name;
        }

        public static bool operator ==(FakeInventoryItemInstance a, FakeInventoryItemInstance b)
        {
            return a?.Metadata.Name == b?.Metadata.Name;
        }

        public override int GetHashCode()
        {
            return Metadata.Name.GetHashCode();
        }
    }

    public class FakeInventoryItemMetadata : IInventoryItemMetadata
    {
        public readonly GameObject ModelPrefab;

        public FakeInventoryItemMetadata(string name, Sprite sprite, GameObject modelPrefab, int maxStack = 1)
        {
            Name = name;
            Sprite = sprite;
            ModelPrefab = modelPrefab;
            MaxStack = maxStack;
        }

        public string Name { get; }
        public Sprite Sprite { get; }
        public int MaxStack { get; }
    }

    public class EquipmentData : IEquipmentItemInstance
    {
        private IEquipmentItemInstance m_EquipmentItemInstanceImplementation;

        public EquipmentData(FakeInventoryItemInstance parent, string mainSlot, string secondarySlot)
        {
            Parent = parent;
            this.mainSlot = mainSlot;
            this.secondarySlot = secondarySlot;
            Sprite = parent.Metadata.Sprite;
            ModelPrefab = parent.Metadata.ModelPrefab;
        }

        public GameObject ModelPrefab { get; }
        public Sprite Sprite { get; }
        public FakeInventoryItemInstance Parent { get; }
        public string mainSlot { get; }
        public string secondarySlot { get; }

        public bool IsStackable => Parent.StorageData.StackableData.Max > 1;
        public IEquipmentItemMetadata Metadata => m_EquipmentItemInstanceImplementation.Metadata;

        public string GetAmmoText => IsStackable ? CurrentAmount.ToString() : string.Empty;

        public int CurrentAmount => Parent.StorageData.StackableData.Current;

        public bool Combine(IEquipmentItemInstance other)
        {
            if (other is not FakeInventoryItemInstance otherItem) return false;

            if (!otherItem.Equals(Parent)) return false;

            if (!IsStackable) return false;

            var amountToAdd = otherItem.EquipmentData.CurrentAmount;

            Parent.StorageData.StackableData.Receive(otherItem.EquipmentData.CurrentAmount,
                out int leftovers);

            otherItem.StorageData.StackableData.Reduce(otherItem.EquipmentData.CurrentAmount - leftovers, out _);

            return true;
        }
    }
}