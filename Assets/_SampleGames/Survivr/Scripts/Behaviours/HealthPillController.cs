using Features.Health;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthPillController : MonoBehaviour
    {
        public int HealValue = 1;

        private bool m_IsExpended;

        public Item_SO ItemMetadata;
        
        private void OnTriggerEnter(Collider other)
        {
            if (m_IsExpended) return;
            
            var character = other.transform.root.GetComponent<CharacterController>();

            if (character == null) return;

            var health = character.GetComponentInChildren<HealthController>();

            if (health == null) return;
            
            m_IsExpended = true;
            
            health.Heal(HealValue);

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