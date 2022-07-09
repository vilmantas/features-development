using Inventory.Abstract.Internal;

namespace Inventory
{
    public class RemoveRequestResult : IChangeRequestResult
    {
        public readonly int AmountRemoved;

        public RemoveRequestResult(RemoveRequest request, bool isSuccess, int amountRemoved)
        {
            RemoveRequest = request;
            IsSuccess = isSuccess;
            AmountRemoved = amountRemoved;
        }

        public RemoveRequest RemoveRequest { get; }
        public IChangeRequest Request => RemoveRequest;

        public bool IsSuccess { get; }
    }
}