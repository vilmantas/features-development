using System.Collections;
using Features.Actions;
using Features.Buffs;
using Features.Conditions;
using Features.Skills;
using Integrations.Actions;
using Integrations.Items;
using Integrations.Skills;
using Integrations.StatusEffects;
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
            
        var payload = LootItem.MakePayload(gameObject, Target, itemInstance);

        controller.DoAction(payload);
    }

    public void GiveStackable()
    {
        var controller = Target.GetComponentInChildren<ActionsController>();

        var itemInstance = Ammo.MakeInstanceWithCount();
            
        var payload = LootItem.MakePayload(gameObject, Target, itemInstance);

        controller.DoAction(payload);
    }

    public void CastLifter()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new(LifterBuff.Metadata, gameObject, 1) {Stacks = 1});
    }

    public void CastShifter()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new BuffAddOptions(ShifterBuff.Metadata, gameObject, 1));
    }

    public void CastHealingReducer()
    {
        var controller = Target.GetComponentInChildren<BuffController>();

        if (controller == null) return;

        controller.AttemptAdd(new(Reducer.Metadata, gameObject, 1) {Stacks = 1});
    }

    public void AttemptHeal()
    {
        var controller = Target.GetComponentInChildren<ActionsController>();

        controller.DoAction(Heal.MakePayload(gameObject, Target, 10));
    }

    public void AddCondition()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var status = new StatusEffectMetadata(nameof(DoingActionStatusEffect));

        var payload = new StatusEffectAddPayload(status);
        
        controller.AddStatusEffect(payload);
    }

    public void RemoveCondition()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var status = new StatusEffectMetadata(nameof(DoingActionStatusEffect));

        var payload = new StatusEffectRemovePayload(status);
        
        controller.RemoveStatusEffect(payload);
    }
    
    public void AddStun()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var status = new StatusEffectMetadata(nameof(StunStatusEffect));

        var payload = new StatusEffectAddPayload(status);
        
        controller.AddStatusEffect(payload);
    }

    public void RemoveStun()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var status = new StatusEffectMetadata(nameof(StunStatusEffect));

        var payload = new StatusEffectRemovePayload(status);
        
        controller.RemoveStatusEffect(payload);
    }
    
    public void MakeDead()
    {
        var controller = Target.GetComponentInChildren<StatusEffectsController>();

        var status = new StatusEffectMetadata(nameof(DeathStatusEffect));

        var payload = new StatusEffectAddPayload(status);
        
        controller.AddStatusEffect(payload);
    }
    
    public void GiveBasicAttackRandomSkill()
    {
        var metadata =
            new SkillMetadata(nameof(BasicAttackSkill), nameof(BasicAttackSkill),
                nameof(BasicAttackSkill), 1f, 1f, SkillTarget.Self);
            
        var controller = Target.GetComponentInChildren<SkillsController>();
        
        controller.Add(metadata);
    }

    public void GiveBasicChannel()
    {
        var controller = Target.GetComponentInChildren<ChannelingController>();

        var channelCommand = new ChannelingCommand("Something" + Random.value, 2f);
        
        controller.StartChanneling(channelCommand);
    }
}
