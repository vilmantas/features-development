using System.Collections.Generic;
using Features.Actions;
using Features.Equipment;
using Features.Inventory;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Character
{
    public class CharacterActionsManager : MonoBehaviour
    {
        private ActionsController m_ActionsController;
        private InventoryController m_InventoryController;
        private EquipmentController m_EquipmentController;
        private Transform Root;
        private GameObject RootGameObject;

        private void Awake()
        {
            Root = transform.root;
            
            RootGameObject = Root.gameObject;

            m_ActionsController = Root.GetComponentInChildren<ActionsController>();

            m_InventoryController = Root.GetComponentInChildren<InventoryController>();

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();
            
            Subscribe();
        }

        private void Subscribe()
        {
            if (m_InventoryController)
            {
                m_InventoryController.OnActionSelected += OnActionSelected;
            }

            if (m_EquipmentController)
            {
                m_EquipmentController.OnUnequipRequested += HandleUnequipRequest;
            }
        }

        private void HandleUnequipRequest(UnequipRequest obj)
        {
            var payload = Unequip.MakePayload(RootGameObject, RootGameObject, obj.ContainerItem);

            m_ActionsController.DoAction(payload);
        }

        private void OnActionSelected(StorageData source, string actionName)
        {
            var item = source.ParentCast<ItemInstance>();

            var action = actionName == Constants.DefaultAction
                ? item.Metadata.Action
                : new ActionBase(actionName);

            var payload = new ActionActivationPayload(action, RootGameObject, RootGameObject,
                new Dictionary<string, object>() {{"item", item}});
            
            m_ActionsController.DoAction(payload);
        }
    }
}