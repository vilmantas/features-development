using System;
using System.Collections;
using System.Collections.Generic;
using Features.Combat;
using UnityEngine;

namespace Feature.Combat
{
    public static class CombatManager
    {
        public static void Attack(CombatController target, AttackMetadataBase attack, Action<AttackResultOld> resultCallback)
        {
            Action<HitMetadataBase> hitCallback = attackResult =>
            {
                resultCallback.Invoke((new AttackResultOld(target, attack, attackResult)));
            };
            
            target.Hit(attack, hitCallback);
        }
    }
}