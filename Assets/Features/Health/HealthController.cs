using System;
using Features.Health.Events;
using UnityEngine;
using Utilities;

namespace Features.Health
{
    public class HealthController : MonoBehaviour
    {
        [Range(1, 100)] [SerializeField] private int StartingHealth = 10;

        [Range(1, 100)] [SerializeField] private int MaximumHealth = 10;

        public DamageReceivedEvent OnDamageReceived = new();

        public HealingReceivedEvent OnHealingReceived = new();

        public HealingAttemptedEvent OnHealingAttempted = new();

        public DamageAttemptedEvent OnDamagingAttempted = new();

        public Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs>
            OnDamagingAttemptedNew;

        public Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs>
            OnHealingAttemptedNew;

        private ResourceContainer Model;

        public int CurrentHealth => Model.Current;

        public int MaxHealth => Model.Max;

        private void Awake()
        {
            Model = new ResourceContainer(MaximumHealth, StartingHealth);
        }

        public void Heal(int amount)
        {
            if (OnHealingAttemptedNew == null)
            {
                Receive(amount);
            }
            else
            {
                var resultAmount = amount;

                foreach (Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs> interceptor in OnHealingAttemptedNew.GetInvocationList())
                {
                    resultAmount = interceptor(new HealthChangeAttemptedEventArgs(this, amount))
                        .NewAmount;
                }

                Receive(resultAmount);
            }
        }

        public void Damage(int amount)
        {
            if (OnDamagingAttemptedNew == null)
            {
                Reduce(amount);
            }
            else
            {
                var resultAmount = amount;

                foreach (Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs> interceptor in OnDamagingAttemptedNew.GetInvocationList())
                {
                    resultAmount = interceptor(new HealthChangeAttemptedEventArgs(this, amount))
                        .NewAmount;
                }

                Reduce(resultAmount);
            }
        }

        private void Reduce(int amount)
        {
            var before = Model.Current;

            if (!Model.Reduce(amount)) return;

            OnDamageReceived.Invoke(new HealthChangeEventArgs(before, Model.Current, amount));
        }

        private void Receive(int amount)
        {
            var before = Model.Current;

            if (!Model.Receive(amount)) return;

            OnHealingReceived.Invoke(new HealthChangeEventArgs(before, Model.Current, amount));
        }
    }
}