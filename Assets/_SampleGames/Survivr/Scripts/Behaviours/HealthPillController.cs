using System;
using Features.Health;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class HealthPillController : MonoBehaviour
    {
        public int HealValue = 1;

        private bool Used = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (Used) return;
            
            var character = other.transform.root.GetComponent<CharacterController>();

            if (character == null) return;

            var health = character.GetComponentInChildren<HealthController>();

            if (health == null) return;
            
            Used = true;
            
            health.Heal(HealValue);

            Destroy(gameObject);
        }
    }
}