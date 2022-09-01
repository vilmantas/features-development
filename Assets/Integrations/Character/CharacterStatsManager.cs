using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
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

            m_EquipmentController.OnItemEquipped += OnItemEquipped;
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