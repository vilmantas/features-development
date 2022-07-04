using System;
using Feature.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Combat
{
    public class CombatController : MonoBehaviour
    {
        [HideInInspector] 
        public OnHitEvent OnHit = new ();
        
        internal void Hit(AttackMetadataBase source, Action<HitMetadataBase> resultCallback)
        {
            OnHit.Invoke(source, resultCallback);
        }
    }
    
    [Serializable]
    public class OnHitEvent : UnityEvent<AttackMetadataBase, Action<HitMetadataBase>> {}
}