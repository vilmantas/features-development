namespace Features.Equipment
{
    public class EquipResult
    {
        public readonly EquipmentContainerItem EquipmentContainerItem;
        public readonly EquipRequest Request;

        public readonly bool Succeeded;
        public readonly bool ItemsCombined;

        public readonly IEquipmentItemInstance UnequippedItem;

        public IEquipmentItemInstance EquippedItem => EquipmentContainerItem.Main;
        
        public EquipResult(EquipRequest request, EquipmentContainerItem equipmentContainerItem,
            IEquipmentItemInstance unequippedItem, bool itemsCombined, bool succeeded)
        {
            Request = request;
            Succeeded = succeeded;
            UnequippedItem = unequippedItem;
            ItemsCombined = itemsCombined;
            EquipmentContainerItem = equipmentContainerItem;
        }

        public EquipResult(EquipRequest request)
        {
            Request = request;
        }
    }
}