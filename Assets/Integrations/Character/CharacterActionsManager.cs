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
        private EquipmentController m_EquipmentController;
        private InventoryController m_InventoryController;
        private Transform Root;
        private GameObject RootGameObject;

        private void Awake()
        {
            Root = transform.root;

            RootGameObject = Root.gameObject;

            m_ActionsController = Root.GetComponentInChildren<ActionsController>();

            m_InventoryController = Root.GetComponentInChildren<InventoryController>();

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();

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