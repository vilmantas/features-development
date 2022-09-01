using System;
using Feature.Combat;
using Features.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DebugScripts
{
    public class CombatDebug : MonoBehaviour
    {
        private CombatController m_CombatController;
        
        [SerializeField]
        public CombatController Target;

        private void Start()
        {
            m_CombatController = GetComponentInChildren<CombatController>();
            
            m_CombatController.OnHit += ((data, callback) =>
            {
                OnHit(data, out var result);
                
                callback.Invoke(result);
            });
        }

        private void BasicAttack(CombatController target)
        {
            CombatManager.Attack(target, DamageCallback(), ResultCallback);
        }

        public void Attack(CombatController target)
        {
            BasicAttack(target);
        }

        public void Attack()
        {
            BasicAttack(Target);
        }

        public void OnHit(AttackMetadataBase attacker, out HitMetadataBase hitMetadataBase)
        {
            hitMetadataBase = null;

            var realSource = attacker as AttackDebug;

            print($"Defending from: {realSource.AttackType}");
            
            if (Random.Range(1, 10) > 5)
            {
                hitMetadataBase = new HitDebug(Random.Range(1, 10));
            }
        }

        private AttackMetadataBase DamageCallback()
        {
            var attackType = "Melee";

            if (Random.Range(1, 10) > 5)
            {
                attackType = "Ranged";
            }
            
            return new AttackDebug(attackType);
        }

        private void ResultCallback(AttackResultOld resultOld)
        {
            print($"{transform.name}: Attack result: {resultOld}");
            
            var h = resultOld.HitMetadataBase as HitDebug;

            var d = resultOld.AttackMetadataBase as AttackDebug;

            print(h != null ? $"Damage: {h.Damage}" : "Miss!");
        }
    }
}