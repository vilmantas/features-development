using System;
using Feature.Combat;
using UnityEngine;

namespace Features.Combat
{
    public interface ICombatManager
    {
        CombatController Controller { get; }
        
        bool TryAvoidAttack(Attack request);
        int OnHit(AttackInfo source);
        AttackInfo DamageCallback();
        void ResultCallback(AttackResult result);
    }
}