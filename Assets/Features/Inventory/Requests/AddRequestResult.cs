using Features.Inventory.Abstract.Internal;

namespace Features.Inventory
{
    public class AddRequestResult : IChangeRequestResult
    {
        public readonly int AmountAdded;

        public AddRequestResult(AddRequest request, bool isSuccess, int amountAdded)
        {
            AddRequest = request;
            IsSuccess = isSuccess;
            AmountAdded = amountAdded;
        }

        public AddRequest AddRequest { get; }

        public bool IsSuccess { get; }
    }
}