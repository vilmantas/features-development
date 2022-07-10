using System.Collections.Generic;
using UnityEngine;

namespace Stats.Unity
{
    [ExecuteAlways]
    public class StatsUI : MonoBehaviour
    {
        public StatsUIData DataUnitPrefab;

        private Dictionary<string, StatsUIData> Datas = new();

        private StatController m_CurrentSource;

        private CanvasGroup m_MainGroup;
        private RectTransform m_UIItemsGrid;

        private void Awake()
        {
            m_MainGroup = GetComponent<CanvasGroup>();

            foreach (Transform c in transform)
            {
                if (c.name != "Items") continue;

                m_UIItemsGrid = c.GetComponent<RectTransform>();
            }
        }

        public void SetSource(StatController source)
        {
            if (m_CurrentSource != null) RemoveCurrentSource();

            m_CurrentSource = source;
            ClearUI();
            DisplayNewUI();

            m_CurrentSource.OnStatsChanged.AddListener(OnStatsChanged);
        }

        public void RemoveCurrentSource()
        {
            m_CurrentSource.OnStatsChanged.RemoveListener(OnStatsChanged);
            ClearUI();
            m_CurrentSource = null;
        }

        private void OnStatsChanged(StatsChangedEventArgs args)
        {
            DisplayNewUI();
        }

        private void ClearUI()
        {
            foreach (var data in Datas)
            {
                Destroy(data.Value.gameObject);
            }

            Datas.Clear();
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_CurrentSource.CurrentStats.Stats)
            {
                UpsertUIData(item.Key, item.Value.Value);
            }
        }

        private void UpsertUIData(string stat, int value)
        {
            if (!Datas.TryGetValue(stat, out StatsUIData data))
            {
                data = Instantiate(DataUnitPrefab, m_UIItemsGrid.transform);

                data.gameObject.SetActive(false);

                data.SetData(stat, value);

                data.gameObject.SetActive(true);

                Datas.Add(stat, data);
            }
            else
            {
                data.SetData(stat, value);
            }
        }

        public void ToggleVisibility(bool value)
        {
            m_MainGroup.alpha = value ? 1 : 0;
        }
    }
}