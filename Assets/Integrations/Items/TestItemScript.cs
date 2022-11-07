using UnityEngine;

namespace Integrations.Items
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
            Debug.Log("Equip");
        }
        
        private static void OnUnequip(GameObject receiver, ItemInstance item)
        {
            Debug.Log("Unequip");
        }
        
        private static void OnReceive(GameObject receiver, ItemInstance item)
        {
            Debug.Log("Receive");
        }

        private static void OnLoss(GameObject receiver, ItemInstance item)
        {
            Debug.Log("Loss");
        }

    }
}