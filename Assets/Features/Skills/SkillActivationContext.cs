using UnityEngine;

namespace Features.Skills
{
    public class SkillActivationContext
    {
        public string Skill => Metadata.ReferenceName;

        public readonly SkillMetadata Metadata;
        
        public readonly GameObject Source;

        public GameObject TargetObject { get; private set; }

        public Vector3 TargetLocation { get; private set; }

        public bool PreventDefault;

        public SkillActivationContext(SkillMetadata metadata, GameObject source)
        {
            Metadata = metadata;
            Source = source;
        }
    }
}