using System;
using System.Collections;
using Features.Actions;
using Features.Buffs;
using Features.Character;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Items;
using Integrations.Actions;
using UnityEngine;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public Modules.Character Character;
        
        public GameObject Target;

        public Buff_SO ShifterBuff;

        public Buff_SO LifterBuff;

        public Buff_SO Reducer;

        public Item_SO Item;

        public Item_SO Ammo;

        private bool DamageEnabled;

        private void Start()
        {
            StartCoroutine(HitterCoroutine());
            
            Character.m_HealthController.OnDamage += OnDamage;
        }

        private void OnDamage(HealthChangeEventArgs obj)
        {
            print("Received damage: " + obj.ActualChange);
        }

        public IEnumerator HitterCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.5f);
                
                Character.Events.OnAttemptStrike.Invoke();
            }
        }

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
    }
}