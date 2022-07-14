using System;
using Features.Health.Events;
using UnityEngine;
using Utilities;

namespace Features.Health
{
    public class HealthController : MonoBehaviour
    {
        [Range(1, 100)] [SerializeField] private int StartingHealth = 10;

        [Range(1, 100)] [SerializeField] private int MaximumHealth = 50;

        public Action<HealthChangeEventArgs> OnDamage;

        public Action<HealthChangeEventArgs> OnHeal;

        public Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs>
            OnBeforeDamage;

        public Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs>
            OnBeforeHeal;

        private ResourceContainer m_Model;

        public int CurrentHealth => m_Model.Current;

        public int MaxHealth => m_Model.Max;

        private void Awake()
        {
            m_Model = new ResourceContainer(MaximumHealth, StartingHealth);
        }

        public void Heal(int amount)
        {
            if (OnBeforeHeal == null)
            {
                Receive(amount);
            }
            else
            {
                var resultAmount = amount;

                foreach (Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs> interceptor in OnBeforeHeal.GetInvocationList())
                {
                    resultAmount = interceptor(new HealthChangeAttemptedEventArgs(this, amount))
                        .NewAmount;
                }

                Receive(resultAmount);
            }
        }

        public void Damage(int amount)
        {
            if (OnBeforeDamage == null)
            {
                Reduce(amount);
            }
            else
            {
                var resultAmount = amount;

                foreach (Func<HealthChangeAttemptedEventArgs, HealthChangeInterceptedEventArgs> interceptor in OnBeforeDamage.GetInvocationList())
                {
                    resultAmount = interceptor(new HealthChangeAttemptedEventArgs(this, amount))
                        .NewAmount;
                }

                Reduce(resultAmount);
            }
        }

        private void Reduce(int amount)
        {
            var before = m_Model.Current;

            if (!m_Model.Reduce(amount)) return;

            OnDamage?.Invoke(new HealthChangeEventArgs(this, before, m_Model.Current, amount));
        }

        private void Receive(int amount)
        {
            var before = m_Model.Current;

            if (!m_Model.Receive(amount)) return;

            OnHeal?.Invoke(new HealthChangeEventArgs(this, before, m_Model.Current, amount));
        }
    }
}