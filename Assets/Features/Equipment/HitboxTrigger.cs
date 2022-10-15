using System;
using UnityEngine;

namespace Features.Equipment
{
    [RequireComponent(typeof(Rigidbody))]
    public class HitboxTrigger : MonoBehaviour
    {
        private Transform root;
        
        public Action<Collider> OnHitboxTriggered;

        private void Awake()
        {
            root = transform.root;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "hitbox") return;
            
            if (other.transform.root == root) return;
            
            OnHitboxTriggered?.Invoke(other);
        }
    }
}