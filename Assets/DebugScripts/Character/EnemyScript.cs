using Features.Buffs;
using Features.Health;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public GameObject Target;

        public Buff_SO ShifterBuff;

        public Buff_SO LifterBuff;

        public Buff_SO Reducer;

        public Item_SO Item;

        public Item_SO Ammo;

        public void GiveItem()
        {
            var controller = Target.GetComponentInChildren<InventoryController>();

            controller.HandleRequest(
                ChangeRequestFactory.Add(Item.MakeInstance().StorageData, 1));
        }

        public void GiveStackable()
        {
            var controller = Target.GetComponentInChildren<InventoryController>();

            controller.HandleRequest(
                ChangeRequestFactory.Add(Ammo.MakeInstance().StorageData));
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
            var controller = Target.GetComponentInChildren<HealthController>();

            controller.Heal(10);
        }

        public void OnReceive(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(Vector3.up * 5);
        }

        public void OnRemove(BuffActivationPayload payload)
        {
            payload.Target.transform.Translate(Vector3.down * 5);
        }
    }
}