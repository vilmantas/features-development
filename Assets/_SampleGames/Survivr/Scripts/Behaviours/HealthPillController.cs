using Features.Actions;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthPillController : MonoBehaviour
    {
        public Item_SO ItemMetadata;
        private bool m_IsExpended;

        private void OnTriggerEnter(Collider other)
        {
            if (m_IsExpended) return;

            var otherRoot = other.transform.root;

            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(LootItem)), gameObject, otherRoot.gameObject);

            var item = ItemMetadata.MakeInstanceWithCount();

            var pickupPayload = new LootItemActionPayload(actionPayload, item);

            otherRoot.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);

            Destroy(gameObject);
        }
    }
}