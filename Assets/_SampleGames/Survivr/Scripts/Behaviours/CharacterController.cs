using System;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Events;
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

        class MyClass : IEquatable<object>
        {
            public MyClass(string name)
            {
                Name = name;
            }

            private string Name { get; set; }

            private bool Equals(MyClass other)
            {
                return Name == other.Name;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MyClass) obj);
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }

        private void Awake()
        {
            m_InventoryController = GetComponentInChildren<InventoryController>();
            
            m_InventoryController.Initialize();
            
            m_InventoryController.OnChangeRequestHandled.AddListener(Wtf);

            m_InventoryController.HandleRequest(ChangeRequestFactory.Add(new StorageData(new MyClass("wtf"), 1)));
            
            m_Agent = GetComponentInChildren<NavMeshAgent>();
            
            UserInputManager.OnGroundClicked += OnGroundClicked;

            m_HealthController = GetComponentInChildren<HealthController>();

            m_HealthController.OnChange += HandleChange;
        }

        private void Wtf(IChangeRequestResult handledEvent)
        {
            print("Handled");
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

        private void OnDestroy()
        {
            UserInputManager.OnGroundClicked -= OnGroundClicked;
            
            m_HealthController.OnChange -= HandleChange;
        }
    }
}