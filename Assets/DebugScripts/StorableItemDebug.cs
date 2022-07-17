using System;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class StorableItemDebug : IEquatable<object>
    {
        public StorageData item;

        public StorableItemDebug()
        {
            item = new StorageData(this, 5);
        }

        public string Name { get; set; }
    }
}