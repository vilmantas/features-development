using Features.Inventory.Abstract.Internal;

namespace Features.Inventory
{
    public class SwapSlotsRequestResult : IChangeRequestResult
    {
        public SwapSlotsRequestResult(SwapSlotsRequest request, bool result)
        {
            Request = request;
            IsSuccess = result;
        }

        public IChangeRequest Request { get; }

        public bool IsSuccess { get; }
    }
}