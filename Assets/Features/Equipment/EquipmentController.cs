using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Equipment
{
    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] private SlotData[] EquipmentSlots;

        private Container m_Container;

        public Action<EquipResult> OnItemEquipped;

        public Action<EquipResult> OnItemCombined;
        
        public Action<EquipmentContainerItem> OnItemUnequipRequested;
        
        public Action<EquipmentContainerItem> OnSlotUpdated;

        public string[] AvailableSlots => EquipmentSlots.Select(x => x.slotType).ToArray();

        public IReadOnlyList<EquipmentContainerItem> ContainerSlots => m_Container.ContainerSlots;

        public void Awake()
        {
            m_Container = new Container(AvailableSlots);
        }

        public void RequestUnequip(EquipmentContainerItem containerItem)
        {
            OnItemUnequipRequested?.Invoke(containerItem);
        }

        public void HandleEquipRequest(EquipRequest request)
        {
            var result = m_Container.Equip(request);

            if (!result.Succeeded) return;

            if (result.ItemsCombined)
            {
                OnItemCombined?.Invoke(result);
            }
            else
            {
                OnItemEquipped?.Invoke(result);
                HandleItemEquipped(result);
            }
        }

        public void NotifyItemChanged(EquipmentContainerItem item)
        {
            if (ContainerSlots.Any(x => x.Id == item.Id))
            {
                OnSlotUpdated?.Invoke(item);
            }
        }

        private void HandleItemEquipped(EquipResult result)
        {
            if (ContainerFor(result.EquipmentContainerItem.Slot).InstanceParent == null) return;

            if (result.UnequippedItemInstanceBase != null)
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

        private void HandleEquip(IEquipmentItemInstance itemInstanceDefinition, string slot)
        {
            if (itemInstanceDefinition.Metadata.ModelPrefab == null) return;

            var container = ContainerFor(slot);

            if (container == null) return;

            container.Instance = Instantiate(itemInstanceDefinition.Metadata.ModelPrefab, container.InstanceParent);
        }

        private SlotData ContainerFor(string slot) => EquipmentSlots.FirstOrDefault(x => x.slotType == slot);
    }
}