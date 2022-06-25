using System;
using Feature.Combat;
using Features.Combat;
using UnityEngine;

namespace DebugScripts
{
    public class CombatDebug : MonoBehaviour, ICombatManager
    {
        [SerializeField]
        public CombatDebug Target;
        
        public CombatController Controller { get; private set; }

        private Action<CombatController> BasicAttack;
        
        private void Awake()
        {
            Controller = new CombatController(this);

            BasicAttack = Controller.CurryAttack(DamageCallback, ResultCallback);
        }

        public void Attack(CombatController target)
        {
            Attack();
        }

        public void Attack()
        {
            BasicAttack.Invoke(Target.Controller);
        }

        public bool TryAvoidAttack(Attack request)
        {
            return false;
        }

        public int OnHit(AttackInfo source)
        {
            return source.Damage;
        }

        public AttackInfo DamageCallback()
        {
            return new AttackInfo() {Damage = 2, Source = Controller};
        }

        public void ResultCallback(AttackResult result)
        {
            print($"{transform.name}: Attack result: {result}");
        }
    }
}