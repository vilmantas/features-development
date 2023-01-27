using UnityEngine;

namespace Features.Skills
{
    [CreateAssetMenu(fileName = "RENAME ME", menuName = "Skills/Skill Group", order = 0)]
    public class SkillGroup_SO : ScriptableObject
    {
        public Skill_SO[] Skills;
    }
}