using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Equipment.Unity
{
    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] private SlotData[] EquipmentSlots;

        public readonly ItemEquippedEvent OnItemEquippedEvent = new();

        public readonly ItemUnequipEvent OnItemUnequipEvent = new();
        private Container m_Container;

        public string[] AvailableSlots => EquipmentSlots.Select(x => x.slotType).ToArray();

        public IReadOnlyList<EquipmentContainerItem> EquippedItems => m_Container.EquippedItems;

        public void Awake()
        {
            m_Container = new Container(AvailableSlots);
        }

        public void RequestUnequip(EquipmentContainerItem slot)
        {
            OnItemUnequipEvent.Invoke(slot);
        }

        public void HandleEquipRequest(EquipRequest request)
        {
            var result = m_Container.Equip(request);

            if (!result.Succeeded) return;

            OnItemEquipped(result);
            OnItemEquippedEvent.Invoke(result);
        }

        private void OnItemEquipped(EquipResult result)
        {
            if (ContainerFor(result.EquipmentContainerItem.Slot).InstanceParent == null) return;

            if (result.UnequippedItemBase != null)
            {
                HandleRemoveEquip(result.EquipmentContainerItem.Slot);
            }

            if (result.EquipmentContainerItem.Main != null)
            {
                HandleEquip(result.EquipmentContainerItem.Main, result.EquipmentContainerItem.Slot);
            }
        }

        private void HandleRemoveEquip(string slot)
        {
            var container = ContainerFor(slot);

            if (container == null || container.Instance == null) return;

            DestroyImmediate(container.Instance);
        }

        private void HandleEquip(IEquipmentItem itemDefinition, string slot)
        {
            if (itemDefinition.ModelPrefab == null) return;

            var container = ContainerFor(slot);

            if (container == null) return;

            container.Instance = Instantiate(itemDefinition.ModelPrefab, container.InstanceParent);
        }

        private SlotData ContainerFor(string slot) => EquipmentSlots.FirstOrDefault(x => x.slotType == slot);
    }
}