using Features.Actions;
using Features.Equipment;
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
        private Transform Root;

        private void Awake()
        {
            Root = transform.root;

            m_ActionsController = Root.GetComponentInChildren<ActionsController>();

            m_InventoryController = Root.GetComponentInChildren<InventoryController>();
            
            Subscribe();
        }

        private void Subscribe()
        {
            if (m_InventoryController)
            {
                m_InventoryController.OnActionSelected += OnActionSelected;
            }
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