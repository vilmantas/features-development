using System;
using System.Collections;
using System.Collections.Generic;
using Feature.Combat.Events;
using Features.Combat;
using UnityEngine;

namespace Feature.Combat
{
    public class CombatController
    {
        private readonly ICombatManager Manager;

        public CombatController(ICombatManager manager)
        {
            Manager = manager;
        }

        public Action<CombatController> CurryAttack(Func<AttackInfo> infoCallback,
            Action<AttackResult> resultCallback)
        {
            return target => Attack(target, infoCallback.Invoke(), resultCallback);
        }

        public void Attack(CombatController target, AttackInfo info, Action<AttackResult> resultCallback)
        {
            var request = new Attack(info, resultCallback);
            
            target.Defend(request);
        }
        
        private void Defend(Attack request)
        {
            if (Manager.TryAvoidAttack(request))
            {
                request.ResultCallback.Invoke(new FailedAttackResult(this));

                return;
            }
            
            var source = request.Info;

            var damage = Manager.OnHit(source);

            var result = new SuccessfulAttackResult(this, damage);
            
            request.ResultCallback.Invoke(result);
        }
    }
}