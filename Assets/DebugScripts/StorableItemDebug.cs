using Utilities;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class StorableItemDebug
    {
        public StorageData item;

        public StorableItemDebug()
        {
            item = new StorageData(this, new ResourceContainer(5));
        }

        public string Name { get; set; }
    }
}