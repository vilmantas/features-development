using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Combat
{
    public class CombatController : MonoBehaviour
    {
        private Dictionary<string, ProjectileController> m_AmmoData;

        public IReadOnlyDictionary<string, ProjectileController> AmmoData => m_AmmoData;

        private void Awake()
        {
            m_AmmoData = new Dictionary<string, ProjectileController>();
        }

        public void SetAmmo(string ammoName, ProjectileController prefab)
        {
            if (m_AmmoData.ContainsKey(ammoName)) return;
            
            m_AmmoData[ammoName] = prefab;
        }

        public void RemoveAmmo(string ammoName)
        {
            m_AmmoData.Remove(ammoName);
        }
    }
}