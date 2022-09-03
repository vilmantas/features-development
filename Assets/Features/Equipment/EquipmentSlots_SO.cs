using UnityEngine;

namespace Features.Equipment
{
    [CreateAssetMenu(fileName = "Empty Equipment Slots", menuName = "Equipment/New Slot Preset", order = 0)]
    public class EquipmentSlots_SO : ScriptableObject
    {
        public SlotData[] Slots = new[] {new SlotData() {slotType = "Main"}};
    }
}