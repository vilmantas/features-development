using Features.Equipment;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
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
            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += HandleEquipmentChanged;

                m_EquipmentController.OnItemUnequipped += HandleEquipmentChanged;

                m_EquipmentController.OnItemCombined += OnItemCombined;

                m_EquipmentController.OnBeforeUnequip += OnBeforeUnequip;
            }

            if (m_InventoryController)
            {
                m_InventoryController.OnBeforeChangeRequest += OnBeforeAddRequest;
            }
        }

        private void OnBeforeAddRequest(ChangeRequest request)
        {
            if (request is not AddRequest addRequest) return;
        }

        private void OnBeforeUnequip(UnequipRequest obj)
        {
            if (obj.ContainerItem.Main is not ItemInstance item) return;

            if (!m_InventoryController.CanReceive(item.StorageData, out int maxAmountToAdd)) return;

            if (maxAmountToAdd < item.CurrentAmount)
            {
                obj.PreventDefault = true;

                var result = m_InventoryController.HandleRequest(
                    ChangeRequestFactory.Add(item.StorageData)) as AddRequestResult;


                item.StorageData.StackableData.Reduce(result.AmountAdded);

                m_EquipmentController.NotifyItemChanged(obj.ContainerItem);
            }
        }

        private void OnItemCombined(EquipResult result)
        {
            if (result.Request.Item is not ItemInstance requestItem) return;

            if (requestItem.StorageData.Current == 0)
            {
                m_InventoryController.HandleRequest(
                    ChangeRequestFactory.RemoveExact(requestItem.StorageData));
            }
            else
            {
                m_InventoryController.NotifyChange();
            }
        }

        private void HandleEquipmentChanged(EquipResult result)
        {
            if (result.EquipmentContainerItem.Main is ItemInstance equippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.RemoveExact(equippedItem.StorageData));
            }

            if (result.UnequippedItem is ItemInstance unequippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.Add(unequippedItem.StorageData));
            }
        }
    }
}