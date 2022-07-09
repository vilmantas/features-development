using System;
using Inventory.Abstract.Internal;

namespace Inventory
{
    public class SwapSlotsRequest : IChangeRequest
    {
        public Guid SourceId;

        public Guid TargetId;
    }
}