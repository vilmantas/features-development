using System;
using UnityEngine;

namespace Integrations.Items
{
    public class ItemScriptImplementation
    {
        public readonly string Name;

        public Action<GameObject, ItemInstance> OnInventoryAddAction;
        
        public Action<GameObject, ItemInstance> OnInventoryRemoveAction;

        public Action<GameObject, ItemInstance> OnEquipAction;
        
        public Action<GameObject, ItemInstance> OnUnequipAction;

        public ItemScriptImplementation(string name)
        {
            Name = name;
        }
    }
}