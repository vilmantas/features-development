using System;
using Features.Inventory.Abstract.Internal;

namespace Features.Inventory
{
    public class SwapSlotsRequest : IChangeRequest
    {
        public Guid SourceId;

        public Guid TargetId;
    }
}