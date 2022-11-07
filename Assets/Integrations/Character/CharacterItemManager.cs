using System;
using Features.Equipment;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Requests;
using Integrations.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterItemManager : MonoBehaviour
    {
        private Transform Root;

        private GameObject RootGameObject;

        private InventoryController m_InventoryController;

        private EquipmentController m_EquipmentController;
        
        private void Awake()
        {
            Root = transform.root;

            RootGameObject = Root.gameObject;

            m_InventoryController = RootGameObject.GetComponentInChildren<InventoryController>();

            m_EquipmentController = RootGameObject.GetComponentInChildren<EquipmentController>();
            
            if (m_InventoryController)
            {
                m_InventoryController.OnChangeRequestHandled += OnChangeRequestHandled;
            }

            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += OnItemEquipped;
                
                m_EquipmentController.OnItemUnequipped += OnItemUnequipped;
            }
        }

        private void OnItemUnequipped(EquipResult obj)
        {
            if (!EquipResultValid(obj, out ItemInstance item)) return;
            
            foreach (var itemScriptDto in item.Metadata.Scripts)
            {
                itemScriptDto.Implementation.OnUnequipAction?.Invoke(RootGameObject, item);
            }
        }

        private void OnItemEquipped(EquipResult obj)
        {
            if (!EquipResultValid(obj, out ItemInstance item)) return;

            foreach (var itemScriptDto in item.Metadata.Scripts)
            {
                itemScriptDto.Implementation.OnEquipAction?.Invoke(RootGameObject, item);
            }
        }

        private static bool EquipResultValid(EquipResult obj, out ItemInstance item)
        {
            item = null;
            
            if (!obj.Succeeded) return false;

            if (obj.ItemsCombined) return false;

            if (obj.EquippedItem is not ItemInstance itemCast) return false;
            
            item = itemCast;
            return true;

        }

        private void OnChangeRequestHandled(IChangeRequestResult obj)
        {
            switch (obj)
            {
                case AddRequestResult {IsSuccess: true} addRequest:
                {
                    var parent = addRequest.AddRequest.Item.ParentCast<ItemInstance>();
            
                    var scripts = parent?.Metadata?.Scripts;

                    if (scripts == null || scripts.Length == 0) return;
            
                    foreach (var itemScriptDto in scripts)
                    {
                        itemScriptDto.Implementation.OnInventoryAddAction?.Invoke(RootGameObject, parent);
                    }

                    break;
                }
                case RemoveRequestResult {IsSuccess: true} removeRequestResult:
                {
                    var parent = removeRequestResult.RemoveRequest.Item.ParentCast<ItemInstance>();
                
                    var scripts = parent?.Metadata?.Scripts;

                    if (scripts == null || scripts.Length == 0) return;
            
                    foreach (var itemScriptDto in scripts)
                    {
                        itemScriptDto.Implementation.OnInventoryRemoveAction?.Invoke(RootGameObject, parent);
                    }

                    break;
                }
                case RemoveExactRequestResult {IsSuccess: true} removeExactRequestResult:
                {
                    var parent = removeExactRequestResult.RemoveRequest.Item
                        .ParentCast<ItemInstance>();
                
                    var scripts = parent?.Metadata?.Scripts;

                    if (scripts == null || scripts.Length == 0) return;
            
                    foreach (var itemScriptDto in scripts)
                    {
                        itemScriptDto.Implementation.OnInventoryRemoveAction?.Invoke(RootGameObject, parent);
                    }

                    break;
                }
            }
        }
    }
}