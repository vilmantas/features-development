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
        private BuffController m_BuffsController;

        private CombatController m_CombatController;

        private EquipmentController m_EquipmentController;

        private HealthController m_HealthController;
        private InventoryController m_InventoryController;

        private StatsController m_StatsController;

        public void DoSetup()
        {
        }
    }
}