using UnityEngine;

namespace Integrations.Items
{
    public class ItemScriptDTO
    {
        public static MissingItemScriptDTO MissingItemScriptDto =
            new("Missing Implementation");
        
        public readonly string Name;

        protected ItemScriptDTO(string name)
        {
            Name = name;
        }

        public ItemScriptImplementation Implementation => ItemScriptRegistry.Registry[Name];

        public static ItemScriptDTO CreateDTO(string name)
        {
            return ItemScriptRegistry.Registry.ContainsKey(name)
                ? new ItemScriptDTO(name)
                : MissingItemScriptDto;
        }
    }

    public class MissingItemScriptDTO : ItemScriptDTO
    {
        public MissingItemScriptDTO(string name) : base(name)
        {
        }
    }
}