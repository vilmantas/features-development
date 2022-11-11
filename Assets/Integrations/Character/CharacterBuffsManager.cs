using System;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Integrations.Items;
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
                m_EquipmentController.OnItemEquipped += HandleEquipmentChanged;
                m_EquipmentController.OnItemUnequipped += HandleEquipmentChanged;
            }
        }

        private void HandleEquipmentChanged(EquipResult result)
        {
            if (result.UnequippedItem is ItemInstance unequippedItemInstanceBase)
            {
                foreach (var buff in unequippedItemInstanceBase.Metadata.Buffs)
                {
                    m_BuffController.AttemptRemove(new() { Buff = buff });
                }
            }

            if (result.EquipmentContainerItem.Main is ItemInstance equipmentItemInstance)
            {
                foreach (var buff in equipmentItemInstance.Metadata.Buffs)
                {
                    m_BuffController.AttemptAdd(new()
                        {Buff = buff, Source = gameObject, Duration = Single.MaxValue, Stacks = 1});
                }
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