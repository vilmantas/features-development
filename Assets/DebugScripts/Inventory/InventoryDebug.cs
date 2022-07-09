using Features.Inventory;
using Inventory;
using Inventory.Abstract.Internal;
using Inventory.Unity;
using UnityEngine;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class InventoryDebug : MonoBehaviour
    {
        public Transform UIContainer;

        public BaseSlotUIData baseSlotUIPrefab;

        public FakeItem_SO Arrows;

        public FakeItem_SO Axe;

        public FakeItem_SO Pickaxe;
        private InventoryController m_InventoryController;

        private void Start()
        {
            InventoryItemFactories.Register(typeof(FakeItemInstance), wtf);

            m_InventoryController = GetComponentInChildren<InventoryController>();

            m_InventoryController.OnChangeRequestHandled.AddListener(ChangeRequestHandled);

            m_InventoryController.WithUI(baseSlotUIPrefab, UIContainer.transform);

            // foreach (var fakeItemSo in Items)
            // {
            //     var request = ChangeRequestFactory.Add(fakeItemSo.GetInstance.StorageData, 2);
            //
            //     m_InventoryController.HandleRequest(request);
            // }
        }

        private StorageData wtf(StorageData arg)
        {
            var itemInstance = arg.Parent as FakeItemInstance;

            return new FakeItemInstance(itemInstance.Metadata).StorageData;
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

    public class FakeItemInstance
    {
        public readonly FakeItemMetadata Metadata;

        public readonly StorageData StorageData;

        public FakeItemInstance(FakeItemMetadata metadata)
        {
            Metadata = metadata;

            StorageData = new StorageData(this, metadata.MaxStack);
        }

        public override bool Equals(object other)
        {
            if (other is not FakeItemInstance b) return false;

            return Metadata.Name.Equals(b.Metadata.Name);
        }

        public static bool operator !=(FakeItemInstance a, FakeItemInstance b)
        {
            return a?.Metadata.Name != b?.Metadata.Name;
        }

        public static bool operator ==(FakeItemInstance a, FakeItemInstance b)
        {
            return a?.Metadata.Name == b?.Metadata.Name;
        }

        public override int GetHashCode()
        {
            return Metadata.Name.GetHashCode();
        }
    }

    public class FakeItemMetadata
    {
        public readonly int MaxStack;
        public readonly string Name;

        public readonly Sprite Sprite;

        public FakeItemMetadata(string name, Sprite sprite, int maxStack = 1)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack;
        }
    }
}