using Features.Inventory;
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

            var inventory = character.GetComponentInChildren<InventoryController>();

            if (inventory)
            {
                var request = ChangeRequestFactory.Add(ItemMetadata.MakeInstance(), 1);

                inventory.HandleRequest(request);
            }

            Destroy(gameObject);
        }
    }
}