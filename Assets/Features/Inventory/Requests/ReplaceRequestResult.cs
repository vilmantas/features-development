using Features.Inventory.Abstract.Internal;

namespace Features.Inventory
{
    public class ReplaceRequestResult : IChangeRequestResult
    {
        public ReplaceRequestResult(ReplaceRequest request, bool isSuccess)
        {
            ReplaceRequest = request;
            IsSuccess = isSuccess;
        }

        public ReplaceRequest ReplaceRequest { get; }
        public ChangeRequest Request => ReplaceRequest;

        public bool IsSuccess { get; }
    }
}