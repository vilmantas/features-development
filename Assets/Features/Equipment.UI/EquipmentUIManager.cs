using System;
using System.Collections.Generic;

namespace Features.Equipment.UI
{
    public class EquipmentUIManager
    {
        private Dictionary<Guid, IEquipmentUIData> Datas = new();

        private Action<IEquipmentUIData> m_DestroyAction;

        private Func<IEquipmentUIData> m_InstantiationFunc;
        private EquipmentController m_Source;

        public void SetSource(EquipmentController controller, Func<IEquipmentUIData> instantiationFunc,
            Action<IEquipmentUIData> destroyAction)
        {
            m_Source = controller;

            m_InstantiationFunc = instantiationFunc;

            m_DestroyAction = destroyAction;

            SubscribeToSource();

            DisplayNewUI();
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_Source.ContainerSlots)
            {
                var data = m_InstantiationFunc();

                data.OnPressed += ActivateItem;

                data.SetData(item);

                Datas.Add(item.Id, data);
            }
        }

        private void UpsertUIData(EquipmentContainerItem equipmentContainerItem)
        {
            if (!Datas.TryGetValue(equipmentContainerItem.Id, out IEquipmentUIData data)) return;

            data.SetData(equipmentContainerItem);
        }

        private void ActivateItem(EquipmentContainerItem arg0)
        {
            m_Source.UnequipItem(new() {ContainerItem = arg0});
        }

        private void OnItemEquippedEventHandler(EquipResult arg0)
        {
            if (!arg0.Succeeded) return;

            UpsertUIData(arg0.EquipmentContainerItem);
        }

        private void SubscribeToSource()
        {
            m_Source.OnItemEquipped += OnItemEquippedEventHandler;
            m_Source.OnSlotUpdated += OnSlotUpdated;
            m_Source.OnItemCombined += OnItemCombined;
        }

        private void OnItemCombined(EquipResult obj)
        {
            OnSlotUpdated(obj.EquipmentContainerItem);
        }

        private void OnSlotUpdated(EquipmentContainerItem obj)
        {
            if (Datas.ContainsKey(obj.Id))
            {
                Datas[obj.Id].SetData(obj);
            }
        }

        private void UnsubscribeFromSource()
        {
            m_Source.OnItemEquipped -= OnItemEquippedEventHandler;
            m_Source.OnSlotUpdated -= OnSlotUpdated;
            m_Source.OnItemCombined -= OnItemCombined;
        }
    }
}