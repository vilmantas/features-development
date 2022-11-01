using System.Collections;
using System.Collections.Generic;
using Features.Actions;
using Features.Buffs;
using Features.Character;
using Features.Conditions;
using Features.Items;
using Integrations.Actions;
using UnityEditor;
using UnityEngine;

public class HelpersScript : MonoBehaviour
{
    public GameObject Target;

    public Buff_SO ShifterBuff;

    public Buff_SO LifterBuff;

    public Buff_SO Reducer;

    public Item_SO Item;

    public Item_SO Ammo;
    
    public void GiveItem()
    {
        var controller = Target.GetComponentInChildren<ActionsController>();

        var itemInstance = Item.MakeInstanceWithCount();
            
        var payload = LootItem.MakePayload(this, Target, itemInstance);

        controller.DoAction(payload);
    }

    public void GiveStackable()
    {
        var controller = Target.GetComponentInChildren<ActionsController>();

        var itemInstance = Ammo.MakeInstanceWithCount();
            
        var payload = LootItem.MakePayload(this, Target, itemInstance);

        controller.DoAction(payload);
    }

    public void CastLifter()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new(LifterBuff.Base, gameObject, 1) {Stacks = 1});
    }

    public void CastShifter()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new BuffAddOptions() {Buff = ShifterBuff.Base, Source = gameObject, Stacks = 1});
    }

    public void CastHealingReducer()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new(Reducer.Base, gameObject, 1) {Stacks = 1});
    }

    public void AttemptHeal()
    {
        var controller = Target.GetComponentInChildren<ActionsController>();

        controller.DoAction(Heal.MakePayload(this, Target, 10));
    }

    public void AddCondition()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var condition = new StatusEffectMetadata(nameof(RandomStatusEffect));
        
        controller.AddCondition(condition);
    }
}
