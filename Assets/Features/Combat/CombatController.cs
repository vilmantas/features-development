using System;
using Feature.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Combat
{
    public class CombatController : MonoBehaviour
    {
        public Action<AttackMetadataBase, Action<HitMetadataBase>> OnHit;
        
        internal void Hit(AttackMetadataBase source, Action<HitMetadataBase> resultCallback)
        {
            OnHit?.Invoke(source, resultCallback);
        }
    }
}