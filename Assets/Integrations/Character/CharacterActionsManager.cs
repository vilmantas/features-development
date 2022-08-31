using Features.Actions;
using Features.Inventory;
using Features.Items;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Character
{
    public class CharacterActionsManager : MonoBehaviour
    {
        private ActionsController m_ActionsController;
        private InventoryController m_InventoryController;

        private void Awake()
        {
            var root = transform.root;

            m_InventoryController = root.GetComponentInChildren<InventoryController>();

            m_ActionsController = root.GetComponentInChildren<ActionsController>();

            m_InventoryController.OnActionSelected += OnActionSelected;
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