using System;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Events;
using Features.Items;
using UnityEngine;
using UnityEngine.AI;
using Utilities.ItemsContainer;

namespace _SampleGames.Survivr
{
    public class CharacterController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;

        private HealthController m_HealthController;

        private InventoryController m_InventoryController;
        
        public Action OnStartedMoving;

        public Action OnStopped;

        public Action OnDeath;

        public float Speed => m_Agent.speed;

        private Vector3 m_PrevVelocity = Vector3.zero;
        
        private void Update()
        {
            var velocity = m_Agent.velocity;
            
            if (velocity != m_PrevVelocity && m_PrevVelocity == Vector3.zero)
            {
                OnStartedMoving?.Invoke();
            }

            if (velocity == Vector3.zero)
            {
                OnStopped?.Invoke();
            }
        }

        private void Awake()
        {
            m_InventoryController = GetComponentInChildren<InventoryController>();
            
            m_InventoryController.OnChangeRequestHandled.AddListener(HandleInventoryChange);
            
            m_InventoryController.OnActionSelected += OnActionSelected;
            
            m_Agent = GetComponentInChildren<NavMeshAgent>();
            
            UserInputManager.OnGroundClicked += OnGroundClicked;

            m_HealthController = GetComponentInChildren<HealthController>();

            m_HealthController.OnChange += HandleChange;
        }

        private void HandleInventoryChange(IChangeRequestResult handledEvent)
        {
            if (handledEvent.Request is not AddRequest req) return;

            var z = req.SourceInventoryItemBase.Parent as ItemInstance;
        }

        private void HandleChange(HealthChangeEventArgs args)
        {
            if (args.After > 0) return;
                
            OnDeath?.Invoke();
        }
        
        private void OnGroundClicked(Vector3 point)
        {
            m_Agent.SetDestination(point);
        }
        
        private void OnActionSelected(StorageData arg1, string arg2)
        {
            var itemInstance = arg1.ParentCast<ItemInstance>();

            if (itemInstance.Metadata.Name != "Healing Potion") return;
            
            m_HealthController.Heal(10);
            
            m_InventoryController.HandleRequest(ChangeRequestFactory.RemoveExact(arg1));
        }

        private void OnDestroy()
        {
            UserInputManager.OnGroundClicked -= OnGroundClicked;
            
            m_HealthController.OnChange -= HandleChange;
        }
    }
}