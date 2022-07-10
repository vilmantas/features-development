using Features.Buffs;
using Features.Combat;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
using Stats.Unity;
using UnityEngine;

namespace Features.Character
{
    public class CharacterManager : MonoBehaviour
    {
        private BuffController m_BuffController;

        private CombatController m_CombatController;

        private EquipmentController m_EquipmentController;

        private HealthController m_HealthController;
        
        private InventoryController m_InventoryController;

        private StatController m_StatController;

        public void DoSetup()
        {
            m_BuffController = GetComponentInChildren<BuffController>();
            m_CombatController = GetComponentInChildren<CombatController>();
            m_EquipmentController = GetComponentInChildren<EquipmentController>();
            m_HealthController = GetComponentInChildren<HealthController>();
            m_InventoryController = GetComponentInChildren<InventoryController>();
            m_StatController = GetComponentInChildren<StatController>();
        }
    }
}