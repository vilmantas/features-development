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

        public void GiveItem()
        {
            var controller = Target.GetComponentInChildren<InventoryController>();

            controller.HandleRequest(
                ChangeRequestFactory.Add(Item.ToMetadata.ToInstance.StorageData, 1));
        }

        public void CastLifter()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(LifterBuff.Base, gameObject);
        }

        public void CastShifter()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(ShifterBuff.Base, gameObject);
        }

        public void CastHealingReducer()
        {
            var controller = Target.GetComponentInChildren<BuffController>();

            if (controller == null) return;

            controller.AttemptAdd(Reducer.Base, gameObject);
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