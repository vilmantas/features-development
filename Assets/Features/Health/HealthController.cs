using Features.Health.Events;
using UnityEngine;
using Utilities;

namespace Features.Health
{
    public class HealthController : MonoBehaviour
    {
        [Range(1, 100)] [SerializeField] private int StartingHealth = 10;

        [Range(1, 100)] [SerializeField] private int MaximumHealth = 10;

        [HideInInspector] public DamageReceived DamageReceived = new();

        [HideInInspector] public HealingReceived HealingReceived = new();

        private ResourceContainer Model;

        public int CurrentHealth => Model.Current;

        public int MaxHealth => Model.Max;

        private void Awake()
        {
            Model = new ResourceContainer(MaximumHealth, StartingHealth);
        }

        public void Reduce(int amount)
        {
            var before = Model.Current;

            if (!Model.Reduce(amount)) return;

            DamageReceived.Invoke(new HealthChangeResult(before, Model.Current, amount));
        }

        public void Receive(int amount)
        {
            var before = Model.Current;

            if (!Model.Receive(amount)) return;

            HealingReceived.Invoke(new HealthChangeResult(before, Model.Current, amount));
        }
    }
}