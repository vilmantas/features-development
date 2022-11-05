using Features.Equipment;
using Features.Stats.Base;
using Integrations.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private EquipmentController m_EquipmentController;
        private StatsController m_StatsController;

        private void Awake()
        {
            var root = transform.root;

            m_StatsController = root.GetComponentInChildren<StatsController>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += OnItemEquipped;
                m_EquipmentController.OnItemUnequipped += OnItemEquipped;
            }
        }

        private void OnItemEquipped(EquipResult result)
        {
            if (result.UnequippedItem is ItemInstance unequippedItemInstanceBase)
            {
                m_StatsController.RemoveStatModifier(unequippedItemInstanceBase.Metadata.EquipStats);
            }

            if (result.EquippedItem is ItemInstance equipmentItemInstance)
            {
                m_StatsController.ApplyStatModifiers(equipmentItemInstance.Metadata.EquipStats);
            }
        }
    }
}