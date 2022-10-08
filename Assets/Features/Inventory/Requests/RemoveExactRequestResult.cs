using Features.Inventory.Abstract.Internal;

namespace Features.Inventory.Requests
{
    public class RemoveExactRequestResult : ChangeRequest
    {
        public readonly int AmountRemoved;

        public readonly bool IsSuccess;

        public readonly RemoveExactRequest RemoveRequest;

        public RemoveExactRequestResult(RemoveExactRequest request, bool isSuccess, int amountRemoved)
        {
            RemoveRequest = request;
            IsSuccess = isSuccess;
            AmountRemoved = amountRemoved;
        }

        public ChangeRequest Request => RemoveRequest;
    }
}