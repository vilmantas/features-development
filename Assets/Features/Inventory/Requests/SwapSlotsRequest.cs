using System;
using Features.Inventory.Abstract.Internal;

namespace Features.Inventory
{
    public class SwapSlotsRequest : ChangeRequest
    {
        public Guid SourceId;

        public Guid TargetId;
    }
}