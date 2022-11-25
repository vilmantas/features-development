using Features.Actions;
using Features.Buffs;
using Integrations.Actions;
using Integrations.Buffs;
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
            
            item.Extras.Add(nameof(OnHitStunItemScript), new OnHitStunStateData());
            
            actions.OnAfterAction += OnAfterAction;
        }

        private static void OnAfterAction(ActionActivation arg1, ActionActivationResult arg2)
        {
            if (arg1.Payload.Action.Name != nameof(Damage)) return;
            
            if (arg2.IsSuccessful.HasValue && arg2.IsFailed) return;
            
            if (arg1.Payload.Data["item"] is not ItemInstance item) return;

            if (!item.Extras.TryGetValue(nameof(OnHitStunItemScript), out var dataRaw)) return;

            if (dataRaw is not OnHitStunStateData data) return;
            
            data.Increment();

            var duration = item.Metadata.UsageStats.GetStat("OnHitStunDuration", 5);
            
            var triggerCount = item.Metadata.UsageStats.GetStat("OnHitStunTriggerCount", 5);

            if (data.Count != triggerCount) return;
            
            var actionController =
                arg1.Payload.Source.GetComponentInChildren<ActionsController>();

            var stunPayload = AddBuff.MakePayloadPassive(arg1.Payload.Source,
                arg1.Payload.Source, BuffMetadata.Get(nameof(Stun)), duration);

            actionController.DoAction(stunPayload);

            data.Reset();
        }

        private class OnHitStunStateData
        {
            public int Count { get; private set; }
            public OnHitStunStateData()
            {
                Count = 0;
            }

            public void Increment() => Count++;

            public void Reset() => Count = 0;
        }
    }
}