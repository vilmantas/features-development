using Features.Health.Events;
using UnityEngine;
using Utilities;

namespace Features.Health
{
    public class HealthController : MonoBehaviour
    {
        [Range(1, 100)] [SerializeField] private int StartingHealth = 10;

        [Range(1, 100)] [SerializeField] private int MaximumHealth = 10;

        [HideInInspector] public DamageReceivedEvent OnDamageReceived = new();

        [HideInInspector] public HealingReceivedEvent OnHealingReceived = new();

        [HideInInspector] public HealingAttemptedEvent OnHealingAttempted;

        [HideInInspector] public DamageAttemptedEvent OnDamagingAttempted = new();

        private ResourceContainer Model;

        public int CurrentHealth => Model.Current;

        public int MaxHealth => Model.Max;

        private void Awake()
        {
            Model = new ResourceContainer(MaximumHealth, StartingHealth);
        }

        public void AttemptHealing(int amount)
        {
            if (OnHealingAttempted.GetPersistentEventCount() == 0)
            {
                Receive(amount);
            }
            else
            {
                OnHealingAttempted.Invoke(new HealthChangeAttemptedEventArgs(this, amount));
            }
        }

        public void AttemptDamaging(int amount)
        {
            if (OnDamagingAttempted.GetPersistentEventCount() == 0)
            {
                Reduce(amount);
            }
            else
            {
                OnDamagingAttempted.Invoke(new HealthChangeAttemptedEventArgs(this, amount));
            }
        }

        public void Reduce(int amount)
        {
            var before = Model.Current;

            if (!Model.Reduce(amount)) return;

            OnDamageReceived.Invoke(new HealthChangeEventArgs(before, Model.Current, amount));
        }

        public void Receive(int amount)
        {
            var before = Model.Current;

            if (!Model.Receive(amount)) return;

            OnHealingReceived.Invoke(new HealthChangeEventArgs(before, Model.Current, amount));
        }
    }
}