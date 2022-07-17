using Features.Equipment;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        private EquipmentController m_EquipmentController;
        private InventoryController m_InventoryController;

        private void Awake()
        {
            var root = transform.root;

            m_InventoryController = root.GetComponentInChildren<InventoryController>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            Subscribe();
        }

        private void Subscribe()
        {
            m_EquipmentController.OnItemEquipped += OnItemEquipped;
            m_EquipmentController.OnItemUnequipRequested += OnItemUnequipRequested;
        }

        private void OnItemEquipped(EquipResult result)
        {
            if (result.EquipmentContainerItem.Main is ItemInstance equippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.RemoveExact(equippedItem.StorageData));
            }

            if (result.Request.ItemInstance is ItemInstance requestItem)
            {
                if (requestItem.StorageData.Current == 0)
                {
                    m_InventoryController.HandleRequest(
                        ChangeRequestFactory.RemoveExact(requestItem.StorageData));
                }
            }

            if (result.UnequippedItemInstanceBase is ItemInstance unequippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.Add(unequippedItem.StorageData));
            }
        }

        private void OnItemUnequipRequested(EquipmentContainerItem containerItem)
        {
            var instance = containerItem.Main as ItemInstance;

            if (instance == null) return;

            if (!m_InventoryController.CanReceive(instance.StorageData, out int maxAmountToAdd)) return;

            if (maxAmountToAdd >= instance.CurrentAmount)
            {
                UnequipFromSlot(containerItem.Slot);
            }
            else
            {
                ReduceWithoutUnequipping(containerItem, instance);
            }
        }

        private void UnequipFromSlot(string slot)
        {
            m_EquipmentController.HandleEquipRequest(new EquipRequest()
                {ItemInstance = null, SlotType = slot});
        }

        private void ReduceWithoutUnequipping(EquipmentContainerItem containerItem, ItemInstance instance)
        {
            var result = m_InventoryController.HandleRequest(
                ChangeRequestFactory.Add(instance.StorageData)) as AddRequestResult;

            instance.StorageData.StackableData.Reduce(result.AmountAdded);

            m_EquipmentController.NotifyItemChanged(containerItem);
        }
    }
}