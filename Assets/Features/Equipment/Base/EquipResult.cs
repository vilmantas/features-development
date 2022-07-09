namespace Equipment
{
    public class EquipResult
    {
        public readonly EquipmentContainerItem EquipmentContainerItem;
        public readonly EquipRequest Request;

        public readonly bool Succeeded;

        public readonly IEquipmentItem UnequippedItemBase;

        public EquipResult(EquipRequest request, EquipmentContainerItem equipmentContainerItem,
            IEquipmentItem unequippedItemBase, bool succeeded)
        {
            Request = request;
            Succeeded = succeeded;
            UnequippedItemBase = unequippedItemBase;
            EquipmentContainerItem = equipmentContainerItem;
        }

        public EquipResult(EquipRequest request)
        {
            Request = request;
        }

        public IEquipmentItem<T> UnequippedItem<T>() where T : class => UnequippedItemBase as IEquipmentItem<T>;
    }
}