using System;
using System.Collections;
using Features.Actions;
using Features.Buffs;
using Features.Character;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Items;
using Integrations.Actions;
using UnityEngine;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public Modules.Character Character;

        private bool DamageEnabled;

        private void Start()
        {
            StartCoroutine(HitterCoroutine());
            
            Character.m_HealthController.OnDamage += OnDamage;
        }

        private void OnDamage(HealthChangeEventArgs obj)
        {
            print("Received damage: " + obj.ActualChange);
        }

        public IEnumerator HitterCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.5f);
                
                Character.m_CombatController.AttemptStrike();
            }
        }
    }
}