using System;
using System.Linq;
using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterStatCalculator : MonoBehaviour
    {
        private const string MAIN_STAT = "Strength";
        
        private StatsController m_Stats;

        private EquipmentController m_Equipment;
        
        private void Awake()
        {
            var root = transform.root;
            
            m_Equipment = root.GetComponentInChildren<EquipmentController>();

            m_Stats = root.GetComponentInChildren<StatsController>();
        }

        public int GetMainDamage()
        {
            var mainSlot =
                m_Equipment.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");
            
            var totalDamage = 0;

            if (mainSlot is {IsEmpty: false, Main: ItemInstance item})
            {
                totalDamage += item.Metadata.UsageStats["Damage"].Value;
            }

            if (m_Stats)
            {
                totalDamage += m_Stats.CurrentStats[MAIN_STAT].Value;
            }

            return totalDamage;
        }
    }
}