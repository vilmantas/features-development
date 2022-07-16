using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        private EquipmentController m_EquipmentController;

        private StatsController m_StatsController;

        private void Awake()
        {
            var root = transform.root;

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_StatsController = root.GetComponentInChildren<StatsController>();

            m_EquipmentController.OnItemEquipped += OnItemEquipped;

            m_EquipmentController.OnItemUnequipRequested += OnItemUnequipRequested;
        }

        private void OnItemUnequipRequested(EquipmentContainerItem containerItem)
        {
            m_EquipmentController.HandleEquipRequest(new EquipRequest()
                {ItemInstance = null, SlotType = containerItem.Slot});
        }

        private void OnItemEquipped(EquipResult result)
        {
            if (result.UnequippedItemInstanceBase is ItemInstance unequippedItemInstanceBase)
            {
                m_StatsController.RemoveStatModifier(unequippedItemInstanceBase.Metadata.Stats);
            }

            if (result.EquipmentContainerItem.Main is ItemInstance equipmentItemInstance)
            {
                m_StatsController.ApplyStatModifiers(equipmentItemInstance.Metadata.Stats);
            }
        }
    }
}