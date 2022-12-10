using UnityEngine;

namespace Features.Skills
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Skills/New Skill", order = 0)]
    public class Skill_SO : ScriptableObject
    {
        public string ImplementationName;
        public string ReferenceName;
        public string DisplayName;
        public float CastTime;
        public float Cooldown;
        public SkillTarget Target;

        public SkillMetadata GetMetadata => SkillMetadataRegistry.Implementations[ReferenceName];
    }

    public enum SkillTarget
    {
        None,
        Self,
        Character,
        Pointer,
        CharacterLocation,
    }
}