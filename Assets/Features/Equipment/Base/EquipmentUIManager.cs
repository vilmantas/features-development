using System;
using System.Collections.Generic;

namespace Features.Equipment
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

                data.OnPressed.AddListener(ActivateItem);

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
            m_Source.RequestUnequip(arg0);
        }

        private void OnItemEquippedEventHandler(EquipResult arg0)
        {
            if (!arg0.Succeeded) return;

            UpsertUIData(arg0.EquipmentContainerItem);
        }

        private void SubscribeToSource()
        {
            m_Source.OnItemEquipped.AddListener(OnItemEquippedEventHandler);
        }

        private void UnsubscribeFromSource()
        {
            m_Source.OnItemEquipped.RemoveListener(OnItemEquippedEventHandler);
        }
    }
}