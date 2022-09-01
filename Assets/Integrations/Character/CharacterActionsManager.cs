using Features.Actions;
using Features.Equipment;
using Features.Inventory;
using Features.Items;
using Integrations.Actions;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Character
{
    public class CharacterActionsManager : MonoBehaviour
    {
        private Transform Root;
        private GameObject RootGameObject;
        
        private ActionsController m_ActionsController;
        private InventoryController m_InventoryController;
        private EquipmentController m_EquipmentController;

        private void Awake()
        {
            Root = transform.root;

            RootGameObject = Root.gameObject;
            
            m_ActionsController = Root.GetComponentInChildren<ActionsController>();

            m_InventoryController = Root.GetComponentInChildren<InventoryController>();

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();
            
            m_InventoryController.OnActionSelected += OnActionSelected;
            
            m_EquipmentController.OnItemUnequipRequested += OnItemUnequipRequested;
        }

        private void OnItemUnequipRequested(EquipmentContainerItem containerSlot)
        {
            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(Unequip)),
                RootGameObject,
                RootGameObject);

            var unequipPayload = new UnequipActionPayload(actionPayload, containerSlot);
            
            m_ActionsController.DoAction(unequipPayload);
        }

        private void OnActionSelected(StorageData source, string actionName)
        {
            var item = source.ParentCast<ItemInstance>();

            var action = actionName == Constants.DefaultAction
                ? item.Metadata.Action
                : new ActionBase(actionName);
            
            m_ActionsController.DoAction(action, item);
        }
    }
}