using Features.Actions;
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
            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += OnItemEquipped;
                
                m_EquipmentController.OnItemCombined += OnItemCombined;
            }
        }

        private void OnItemCombined(EquipResult result)
        {
            if (result.Request.ItemInstance is not ItemInstance requestItem) return;
            
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

        private void OnItemEquipped(EquipResult result)
        {
            if (result.EquipmentContainerItem.Main is ItemInstance equippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.RemoveExact(equippedItem.StorageData));
            }

            if (result.UnequippedItemInstanceBase is ItemInstance unequippedItem)
            {
                m_InventoryController.HandleRequest(ChangeRequestFactory.Add(unequippedItem.StorageData));
            }
        }
    }
}