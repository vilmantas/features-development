using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Skills
{
    public class SkillsController : MonoBehaviour
    {
        private List<SkillInstance> m_Skills = new();

        public IReadOnlyList<SkillMetadata> Skills => m_Skills.Select(x => x.Metadata).ToList();

        public void Initialize(IEnumerable<SkillMetadata> skills)
        {
            if (m_Skills == null) return;

            m_Skills = skills.Select(x => x.MakeInstance).ToList();
        }
    }
}