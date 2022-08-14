using System;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterBuffsManager : MonoBehaviour
    {
        private ActionsController m_ActionsController;
        private BuffController m_BuffController;

        private EquipmentController m_EquipmentController;

        public void Awake()
        {
            var root = transform.root;

            m_BuffController = root.GetComponentInChildren<BuffController>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_ActionsController = root.GetComponentInChildren<ActionsController>();

            Subscribe();
        }

        private void Subscribe()
        {
            m_BuffController.OnBeforeBuffAdd += HandleBuffAddRequest;

            m_BuffController.OnBuffAdded += HandleBuffAdded;
            m_BuffController.OnBuffRemoved += HandleBuffRemoved;

            m_BuffController.OnBuffTickOccurred += HandleBuffTick;
            m_BuffController.OnBuffDurationReset += HandleBuffDurationReset;

            if (m_EquipmentController)
            {
                m_EquipmentController.OnItemEquipped += HandleItemEquipped;
            }

            if (m_ActionsController)
            {
                m_ActionsController.OnActionActivated += OnActionActivated;
            }
        }

        private void OnActionActivated(ActionActivation obj)
        {
            if (obj.Payload.Source is not ItemInstance item) return;

            foreach (var metadataBuff in item.Metadata.Buffs)
            {
                m_BuffController.AttemptAdd(new BuffAddOptions(metadataBuff, transform.root.gameObject, 1));
            }
        }

        private void HandleItemEquipped(EquipResult obj)
        {
            if (obj.EquipmentContainerItem.IsEmpty) return;

            if (obj.EquipmentContainerItem.Main is not ItemInstance instance) return;

            foreach (var buff in instance.Metadata.Buffs)
            {
                m_BuffController.AttemptAdd(new()
                    {Buff = buff, Source = gameObject, Duration = Single.MaxValue, Stacks = 1});
            }
        }

        private void HandleBuffDurationReset(ActiveBuff buff)
        {
            BuffActivationHelper.Activate(buff, impl => impl.OnDurationReset, transform.root.gameObject);
        }

        private void HandleBuffTick(ActiveBuff buff)
        {
            BuffActivationHelper.Activate(buff, impl => impl.OnTick, transform.root.gameObject);
        }

        private void HandleBuffAddRequest(BuffAddOptions opt)
        {
            opt.RequestHandled = false;
        }

        private void HandleBuffRemoved(ActiveBuff buff)
        {
            BuffActivationHelper.Activate(buff, impl => impl.OnRemove, transform.root.gameObject);
        }

        private void HandleBuffAdded(ActiveBuff buff)
        {
            BuffActivationHelper.Activate(buff, impl => impl.OnReceive, transform.root.gameObject);
        }
    }
}