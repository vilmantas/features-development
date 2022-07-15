namespace Features.Equipment
{
    public class EquipResult
    {
        public readonly EquipmentContainerItem EquipmentContainerItem;
        public readonly EquipRequest Request;

        public readonly bool Succeeded;

        public readonly IEquipmentItemInstance UnequippedItemInstanceBase;

        public EquipResult(EquipRequest request, EquipmentContainerItem equipmentContainerItem,
            IEquipmentItemInstance unequippedItemInstanceBase, bool succeeded)
        {
            Request = request;
            Succeeded = succeeded;
            UnequippedItemInstanceBase = unequippedItemInstanceBase;
            EquipmentContainerItem = equipmentContainerItem;
        }

        public EquipResult(EquipRequest request)
        {
            Request = request;
        }

        public IEquipmentItemInstance<T> UnequippedItem<T>() where T : class => UnequippedItemInstanceBase as IEquipmentItemInstance<T>;
    }
}