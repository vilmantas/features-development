using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using Features.Items;
using Integrations.Actions;
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

            var character = other.transform.root.GetComponent<CharacterController>();

            if (character == null) return;

            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(LootItem)), this, other.gameObject);

            var item = ItemMetadata.MakeInstanceWithCount();

            var pickupPayload = new LootItemActionPayload(actionPayload, item);

            character.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);

            Destroy(gameObject);
        }
    }
}