using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Equipment.Unity
{
    public class UI : MonoBehaviour
    {
        private readonly Dictionary<Guid, UIData> SourceSlots = new();

        private readonly List<UIData> SupportedSlots = new();

        private float DataHeight;

        private float DataWidth;

        private EquipmentController m_CurrentSource;

        private CanvasGroup m_MainGroup;
        private RectTransform m_UIItemsGrid;

        private void Awake()
        {
            m_MainGroup = GetComponent<CanvasGroup>();

            foreach (Transform c in transform)
            {
                if (c.name != "Items") continue;

                m_UIItemsGrid = c.GetComponent<RectTransform>();

                var datas = m_UIItemsGrid.GetComponentsInChildren<UIData>();

                foreach (var data in datas)
                {
                    data.EquipmentButtonPressEvent.AddListener(RemoveEquip);
                    SupportedSlots.Add(data);
                }
            }
        }

        public void SetSource(EquipmentController source)
        {
            if (m_CurrentSource != null) RemoveCurrentSource();

            m_CurrentSource = source;
            DisplayNewUI();

            m_CurrentSource.OnItemEquippedEvent.AddListener(OnEquipmentChanged);
        }

        public void RemoveCurrentSource()
        {
            m_CurrentSource.OnItemEquippedEvent.RemoveListener(OnEquipmentChanged);
            ClearUI();
            m_CurrentSource = null;
        }

        private void OnEquipmentChanged(EquipResult result)
        {
            if (!result.Succeeded) return;

            UpsertUIData(result.EquipmentContainerItem);
        }

        private void ClearUI()
        {
            foreach (var data in SupportedSlots)
            {
                data.Hide();
            }

            SourceSlots.Clear();
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_CurrentSource.EquippedItems)
            {
                var z = SupportedSlots.FirstOrDefault(x => !x.IsVisible() && x.SlotType == item.Slot);

                if (z == null) continue;

                z.Show();

                SourceSlots.Add(item.Id, z);

                UpsertUIData(item);
            }

            foreach (var uiDataV2 in SupportedSlots.Where(x => !x.gameObject.activeInHierarchy))
            {
                uiDataV2.Hide();
            }
        }

        private void UpsertUIData(EquipmentContainerItem equipmentContainerItem)
        {
            if (!SourceSlots.TryGetValue(equipmentContainerItem.Id, out UIData data)) return;

            data.Hide();

            data.SetData(equipmentContainerItem);

            data.Show();
        }

        public void ToggleVisibility(bool value)
        {
            m_MainGroup.alpha = value ? 1 : 0;
            m_MainGroup.blocksRaycasts = !m_MainGroup.blocksRaycasts;
        }

        private void RemoveEquip(EquipmentContainerItem equipmentContainerSlot)
        {
            m_CurrentSource.RequestUnequip(equipmentContainerSlot);
        }
    }
}