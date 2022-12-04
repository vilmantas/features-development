using UnityEngine;

namespace Features.Skills
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Skills/New Skill", order = 0)]
    public class Skill_SO : ScriptableObject
    {
        public string InternalName;
        public string DisplayName;
        public float CastTime;
        public float Cooldown;

        public SkillMetadata Metadata =>
            new(InternalName, DisplayName, CastTime, Cooldown);
    }
}