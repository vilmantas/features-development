using System;
using Features.Buffs;
using Features.Combat;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
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

        public void DoSetup()
        {
            var root = transform.root;

            m_BuffController = root.GetComponentInChildren<BuffController>();
            m_CombatController = root.GetComponentInChildren<CombatController>();
            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();
            m_HealthController = root.GetComponentInChildren<HealthController>();
            m_InventoryController = root.GetComponentInChildren<InventoryController>();
            m_StatsController = root.GetComponentInChildren<StatsController>();

            m_BuffController.OnBuffAddRequested.AddListener(HandleBuffAddRequest);

            m_BuffController.OnBuffAdded.AddListener(HandleBuffAdded);
            m_BuffController.OnBuffRemoved.AddListener(HandleBuffRemoved);

            m_BuffController.OnBuffTickOccurred.AddListener(HandleBuffTick);
            m_BuffController.OnBuffDurationReset.AddListener(HandleBuffDurationReset);
        }

        private void HandleBuffDurationReset(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnDurationReset);
        }

        private void HandleBuffTick(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnTick);
        }

        private void HandleBuffAddRequest(BuffBase buff, GameObject source)
        {
            m_BuffController.Add(buff, source);
        }

        private void HandleBuffRemoved(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnRemove);
        }

        private void HandleBuffAdded(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnReceive);
        }

        private void ActivateBuff(ActiveBuff buff, Func<BuffImplementation, Action<BuffActivationPayload>> action)
        {
            if (!ImplementationRegistered(buff, out var implementation)) return;

            var payload = new BuffActivationPayload(buff.Source, transform.root.gameObject, buff);

            action(implementation)?.Invoke(payload);
        }

        private static bool ImplementationRegistered(ActiveBuff buff, out BuffImplementation implementation)
        {
            if (!BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                    out implementation))
            {
                Debug.Log($"Implementation missing for buff: {buff.Metadata.Name}");
            }

            return BuffImplementationRegistry.Implementations.TryGetValue(buff.Metadata.Name,
                out implementation);
        }
    }
}