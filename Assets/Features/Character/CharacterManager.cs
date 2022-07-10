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

            m_BuffController.OnBuffAddRequested.AddListener(HandleBuffAddRequest);

            m_BuffController.OnBuffAdded.AddListener(HandleBuffAdded);
            m_BuffController.OnBuffRemoved.AddListener(HandleBuffRemoved);
        }

        private void HandleBuffAddRequest(BuffBase buff, GameObject source)
        {
            m_BuffController.Add(buff, source);
        }

        private void HandleBuffRemoved(ActiveBuff buff)
        {
            if (!ImplementationRegistered(buff, out var implementation)) return;

            var payload = new BuffActivationPayload(buff.Source, gameObject, buff);

            implementation.OnRemove(payload);
        }

        private void HandleBuffAdded(ActiveBuff buff)
        {
            if (!ImplementationRegistered(buff, out var implementation)) return;

            var payload = new BuffActivationPayload(buff.Source, gameObject, buff);

            implementation.OnReceive(payload);
        }

        private static bool ImplementationRegistered(ActiveBuff buff, out BuffImplementation implementation)
        {
            if (BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                    out implementation))
            {
                Debug.Log($"Implementation missing for buff: {buff.Metadata.Name}");
            }

            return BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                out implementation);
        }
    }
}