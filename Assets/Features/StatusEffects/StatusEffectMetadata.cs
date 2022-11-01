namespace Features.Conditions
{
    public class StatusEffectMetadata
    {
        public string DisplayName;

        public string InternalName;

        public StatusEffectMetadata(string internalName, string displayName = "")
        {
            InternalName = internalName;

            if (string.IsNullOrEmpty(displayName))
            {
                DisplayName = internalName;   
            }
        }
    }
}