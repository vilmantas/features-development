namespace Features.Conditions
{
    public class StatusEffectMetadata
    {
        public readonly string DisplayName;

        public readonly string InternalName;

        public StatusEffectMetadata(string internalName, string displayName = "")
        {
            InternalName = internalName;

            if (string.IsNullOrEmpty(displayName))
            {
                DisplayName = internalName;   
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not StatusEffectMetadata metadata) return false;

            return metadata.InternalName.Equals(InternalName);
        }

        public override int GetHashCode()
        {
            return InternalName.GetHashCode();
        }
    }
}