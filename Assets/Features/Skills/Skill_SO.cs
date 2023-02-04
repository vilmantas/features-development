using System;
using UnityEngine;

namespace Features.Skills
{
    [CreateAssetMenu(fileName = "Rename Me", menuName = "Skills/New Skill", order = 0)]
    public class Skill_SO : ScriptableObject
    {
        public string ImplementationName;
        public string ReferenceName;
        public string DisplayName;
        public float ChannelingTime;
        public float Cooldown;
        public SkillTarget Target;
        public SkillFlags Flags;
        public ExtraData[] Extras = Array.Empty<ExtraData>();

        public SkillMetadata GetMetadata => SkillMetadataRegistry.Implementations[ReferenceName];
    }

    [Serializable]
    public class ExtraData
    {
        public string Title;

        public float NumericValue;

        public string StringValue;
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