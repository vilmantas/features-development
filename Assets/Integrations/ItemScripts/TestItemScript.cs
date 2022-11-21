using Integrations.Items;
using UnityEngine;

namespace Integrations.ItemScripts
{
    public static class TestItemScript
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Register()
        {
            ItemScriptImplementation implementation = new(nameof(TestItemScript))
            {
                OnEquipAction = OnEquip,
                OnUnequipAction = OnUnequip,
                OnInventoryAddAction = OnReceive,
                OnInventoryRemoveAction = OnLoss,
            };

            ItemScriptRegistry.Register(implementation);
        }

        private static void OnEquip(GameObject receiver, ItemInstance item)
        {
        }
        
        private static void OnUnequip(GameObject receiver, ItemInstance item)
        {
        }
        
        private static void OnReceive(GameObject receiver, ItemInstance item)
        {
        }

        private static void OnLoss(GameObject receiver, ItemInstance item)
        {
        }

    }
}