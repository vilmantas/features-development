namespace Features.Inventory.Abstract.Internal
{
    public interface IChangeRequestResult
    {
        IChangeRequest Request { get; }

        bool IsSuccess { get; }
    }
}