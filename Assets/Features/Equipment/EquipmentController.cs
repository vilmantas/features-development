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

        public Action<EquipRequest> OnBeforeEquip;

        public Action<UnequipRequest> OnBeforeUnequip;

        public Action<EquipResult> OnItemCombined;

        public Action<EquipResult> OnItemEquipped;

        public Action<EquipResult> OnItemUnequipped;

        public Action<EquipmentContainerItem> OnSlotUpdated;

        public Action<string, Collider> OnHitboxCollided;
        
        public string[] AvailableSlotNames => EquipmentSlots.Select(x => x.slotType).ToArray();

        public IReadOnlyList<EquipmentContainerItem> ContainerSlots => m_Container.ContainerSlots;

        public void Awake()
        {
            m_Container = new Container(EquipmentSlots == null ? new string[] { } : AvailableSlotNames);
        }

        public void Initialize(SlotData[] slots)
        {
            EquipmentSlots = slots;

            var slotNames = EquipmentSlots == null ? new string[] { } : AvailableSlotNames;

            m_Container = new Container(slotNames);

            if (EquipmentSlots == null) return;
            
            foreach (var equipmentSlot in EquipmentSlots)
            {
                if (!equipmentSlot.AddHitboxTrigger || equipmentSlot.InstanceParent == null) continue;

                var trigger = equipmentSlot.InstanceParent.gameObject.AddComponent<HitboxTrigger>();

                trigger.OnHitboxTriggered += x => OnHitboxCollided?.Invoke(equipmentSlot.slotType, x);
            }
        }

        public void UnequipItem(UnequipRequest request)
        {
            if (OnBeforeUnequip != null)
            {
                foreach (var boxed in OnBeforeUnequip.GetInvocationList())
                {
                    if (boxed is not Action<UnequipRequest> castDel) continue;

                    castDel.Invoke(request);
                }
            }

            if (request.PreventDefault) return;

            UnequipItem(request.ContainerItem.Id);
        }

        public void HandleEquipRequest(EquipRequest request)
        {
            if (OnBeforeEquip != null)
            {
                foreach (var @delegate in OnBeforeEquip.GetInvocationList())
                {
                    if (@delegate is not Action<EquipRequest> castedDel) continue;

                    castedDel.Invoke(request);
                }
            }

            if (request.PreventDefault) return;

            EquipItem(request);
        }

        private void EquipItem(EquipRequest request)
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
                HandleEquipmentChange(result);
            }
        }

        private void UnequipItem(Guid slotId)
        {
            var request = new EquipRequest() {SlotId = slotId};
            
            var result = m_Container.Equip(request);

            if (!result.Succeeded) return;

            OnItemUnequipped?.Invoke(result);
            HandleEquipmentChange(result);
        }

        public void NotifyItemChanged(EquipmentContainerItem containerSlot)
        {
            if (ContainerSlots.Any(x => x.Id == containerSlot.Id))
            {
                OnSlotUpdated?.Invoke(containerSlot);
            }
        }

        private void HandleEquipmentChange(EquipResult result)
        {
            if (ContainerFor(result.EquipmentContainerItem.Slot).InstanceParent == null) return;

            if (result.UnequippedItem != null)
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

        public Vector3 SpawnPositionForSlot(string slot)
        {
            var slotData = EquipmentSlots.FirstOrDefault(x => x.slotType.ToLower() == slot);
            
            return slotData == null ? Vector3.zero : slotData.Instance.transform.position;
        }

        public IEquipmentItemInstance ItemInSlot(string slot)
        {
            return ContainerSlots.FirstOrDefault(x => x.Slot.ToLower() == slot)?.Main;
        }
    }
}