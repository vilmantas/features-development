namespace Features.Inventory.Abstract.Internal
{
    public interface IChangeRequestResult
    {
        ChangeRequest Request { get; }

        bool IsSuccess { get; }
    }
}