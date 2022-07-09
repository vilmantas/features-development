using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public interface IInventorySlotUI
    {
        InventoryButtonPressEvent OnPressed { get; }

        InventorySlotDraggedEvent OnDragged { get; }

        GameObject gameObject { get; }
        void SetData(ContainerItem slot);
    }
}