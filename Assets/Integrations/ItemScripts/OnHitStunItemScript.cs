using Features.Actions;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace Integrations.ItemScripts
{
    public static class OnHitStunItemScript
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Register()
        {
            ItemScriptImplementation implementation = new(nameof(OnHitStunItemScript))
            {
                OnEquipAction = OnEquip,
                OnUnequipAction = OnUnequip,
            };

            ItemScriptRegistry.Register(implementation);
        }

        private static void OnUnequip(GameObject arg1, ItemInstance arg2)
        {
            Debug.Log("Unsubscribe");
        }

        private static void OnEquip(GameObject receiver, ItemInstance item)
        {
            var actions = receiver.GetComponentInChildren<ActionsController>();
            
            actions.OnAfterAction += OnAfterAction;
        }

        private static void OnAfterAction(ActionActivation arg1, ActionActivationResult arg2)
        {
            if (arg1.Payload.Action.Name != nameof(Damage)) return;
            
            if (arg1.Payload.Data["item"] is not ItemInstance item) return; 
            
            Debug.Log(arg1.Payload.Action.Name);
            
            Debug.Log("STUN TIME");
        }
    }
}