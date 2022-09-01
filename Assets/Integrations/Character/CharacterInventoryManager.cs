using Features.Actions;
using Features.Equipment;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        private ActionsController m_ActionsController;
        private EquipmentController m_EquipmentController;

        private InventoryController m_InventoryController;

        private void Awake()
        {
            var root = transform.root;

            m_InventoryController = root.GetComponentInChildren<InventoryController>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_ActionsController = root.GetComponentInChildren<ActionsController>();

            Subscribe();
        }

        private void Subscribe()
        {
            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += OnItemEquipped;
            }
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
    }
}