using System;
using UnityEngine;

namespace Features.Skills
{
    public class SkillTargetProvider : MonoBehaviour
    {
        private Func<SkillTarget, Vector3, GameObject> m_Provider;
        
        public void Initialize(Func<SkillTarget, Vector3, GameObject> provider)
        {
            m_Provider = provider;
        }
    }
}