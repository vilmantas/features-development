using Features.Buffs;
using Features.Combat;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
using Features.Items;
using Features.Stats.Base;
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

        private StatsController m_StatsController;

        private void Awake()
        {
            ResolveSystems(transform.root);
        }

        private void ResolveSystems(Transform root)
        {
            m_BuffController = root.GetComponentInChildren<BuffController>();
            m_CombatController = root.GetComponentInChildren<CombatController>();
            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();
            m_HealthController = root.GetComponentInChildren<HealthController>();
            m_InventoryController = root.GetComponentInChildren<InventoryController>();
            m_StatsController = root.GetComponentInChildren<StatsController>();
        }
    }
}