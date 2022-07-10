using System.Collections.Generic;
using System.Linq;
using Features.Equipment.Events;
using UnityEngine;

namespace Features.Equipment
{
    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] private SlotData[] EquipmentSlots;

        public readonly ItemEquippedEvent OnItemEquipped = new();

        public readonly ItemUnequipEvent OnItemUnequipRequested = new();

        private Container m_Container;

        private EquipmentUIManager UIManager;

        public string[] AvailableSlots => EquipmentSlots.Select(x => x.slotType).ToArray();

        public IReadOnlyList<EquipmentContainerItem> ContainerSlots => m_Container.ContainerSlots;

        public void Awake()
        {
            m_Container = new Container(AvailableSlots);
        }

        public void WithUI(IEquipmentUIData prefab, Transform container)
        {
            UIManager = new EquipmentUIManager();

            UIManager.SetSource(this,
                () =>
                {
                    var instance = Instantiate(prefab.gameObject, container);
                    return instance.GetComponentInChildren<IEquipmentUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }

        public void RequestUnequip(EquipmentContainerItem slot)
        {
            OnItemUnequipRequested.Invoke(slot);
        }

        public void HandleEquipRequest(EquipRequest request)
        {
            var result = m_Container.Equip(request);

            if (!result.Succeeded) return;

            HandleItemEquipped(result);
            OnItemEquipped.Invoke(result);
        }

        private void HandleItemEquipped(EquipResult result)
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