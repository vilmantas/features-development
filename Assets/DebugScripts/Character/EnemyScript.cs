using System;
using System.Collections;
using Features.Actions;
using Features.Buffs;
using Features.Character;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Movement;
using Integrations.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DebugScripts.Character
{
    public class EnemyScript : MonoBehaviour
    {
        public Modules.Character Character;

        private bool DamageEnabled;

        private Vector3 SpawnPoint;

        private Vector3 RoamTarget;
        
        private void Start()
        {
            var transform1 = transform;
            SpawnPoint = transform1.position;

            RoamTarget = SpawnPoint + (transform1.forward * 5);
            
            StartCoroutine(HitterCoroutine());

            StartCoroutine(MoverCoroutine());
            
            Character.m_HealthController.OnDamage += OnDamage;
        }

        private void OnDamage(HealthChangeEventArgs obj)
        {
        }

        public IEnumerator HitterCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(2, 5));

                var o = gameObject;
                var strikePayload = new ActionActivationPayload(new ActionBase(nameof(Strike)),
                    o, o);

                Character.m_ActionsController.DoAction(strikePayload);
            }
        }

        public IEnumerator MoverCoroutine()
        {
            var moveBack = false;
            
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5, 10));

                Vector3 dest;
                
                if (moveBack)
                {
                    dest = SpawnPoint;
                    
                    moveBack = false;
                }
                else
                {
                    dest = RoamTarget;

                    moveBack = true;
                }
                
                var o = gameObject;

                var movePayload = Move.MakePayload(o, new MoveActionData(dest));

                Character.m_ActionsController.DoAction(movePayload);
            }
        }
    }
}