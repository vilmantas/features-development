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

        private void OnActionSelected(StorageData arg1, string arg2)
        {
            var item = arg1.ParentCast<ItemInstance>();

            if (!arg2.Equals("Click")) return;

            if (item.Metadata.Action == null) return;

            m_ActionsController.DoAction(item.Metadata.Action, item);
        }
    }
}