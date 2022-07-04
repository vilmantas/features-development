using System;
using System.Collections;
using System.Collections.Generic;
using Features.Combat;
using UnityEngine;

namespace Feature.Combat
{
    public static class CombatManager
    {
        public static void Attack(CombatController target, AttackMetadataBase metadataBase, Action<AttackResult> resultCallback)
        {
            Action<HitMetadataBase> hitCallback = b =>
            {
                resultCallback.Invoke((new AttackResult(target, metadataBase, b)));
            };
            
            target.Hit(metadataBase, hitCallback);
        }
    }
}