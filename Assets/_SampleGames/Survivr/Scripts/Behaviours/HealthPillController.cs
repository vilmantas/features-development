using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using Features.Items;
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

            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(PickupItem)), this, other.gameObject);

            var item = ItemMetadata.MakeInstance();

            item.StorageData.StackableData.Receive(ItemMetadata.Count);

            var pickupPayload = new PickupItemActionPayload(actionPayload, item);

            character.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);

            Destroy(gameObject);
        }
    }
}