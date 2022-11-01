namespace Features.Conditions
{
    public class StatusCondition
    {
        public string DisplayName;

        public string InternalName;

        public StatusCondition(string internalName, string displayName = "")
        {
            InternalName = internalName;

            if (string.IsNullOrEmpty(displayName))
            {
                DisplayName = internalName;   
            }
        }
    }
}