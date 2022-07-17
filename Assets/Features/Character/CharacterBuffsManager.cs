using System;
using Features.Buffs;
using Features.Equipment;
using Features.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterBuffsManager : MonoBehaviour
    {
        private BuffController m_BuffController;

        private EquipmentController m_EquipmentController;

        public void Awake()
        {
            var root = transform.root;

            m_BuffController = root.GetComponentInChildren<BuffController>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            Subscribe();
        }

        private void Subscribe()
        {
            m_BuffController.OnBeforeBuffAdd += HandleBuffAddRequest;

            m_BuffController.OnBuffAdded += HandleBuffAdded;
            m_BuffController.OnBuffRemoved += HandleBuffRemoved;

            m_BuffController.OnBuffTickOccurred += HandleBuffTick;
            m_BuffController.OnBuffDurationReset += HandleBuffDurationReset;

            m_EquipmentController.OnItemEquipped += HandleItemEquipped;
        }

        private void HandleItemEquipped(EquipResult obj)
        {
            if (obj.EquipmentContainerItem.IsEmpty) return;

            if (obj.EquipmentContainerItem.Main is not ItemInstance instance) return;

            foreach (var buff in instance.Metadata.Buffs)
            {
                m_BuffController.AttemptAdd(new () { Buff = buff, Source = gameObject, Duration = Single.MaxValue, Stacks = 1 });
            }
        }

        private void HandleBuffDurationReset(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnDurationReset);
        }

        private void HandleBuffTick(ActiveBuff buff)
        {
            ActivateBuff(buff, impl => impl.OnTick);
        }

        private void HandleBuffAddRequest(BuffAddOptions opt)
        {
            opt.RequestHandled = false;
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