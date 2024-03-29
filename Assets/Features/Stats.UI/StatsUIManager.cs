using System;
using System.Collections.Generic;

namespace Features.Stats.Base
{
    public class StatsUIManager
    {
        private Dictionary<string, IStatUIData> Datas = new();

        private Action<IStatUIData> m_DestroyAction;

        private Func<IStatUIData> m_InstantiationFunc;
        private StatsController m_Source;

        public void SetSource(StatsController source, Func<IStatUIData> instantiationFunc,
            Action<IStatUIData> destroyAction)
        {
            m_Source = source;

            m_InstantiationFunc = instantiationFunc;
            m_DestroyAction = destroyAction;

            m_Source.OnStatsChanged += OnStatsChanged;

            ClearUI();

            DisplayNewUI();
        }

        public void RemoveCurrentSource()
        {
            m_Source.OnStatsChanged -= OnStatsChanged;
            ClearUI();
            m_Source = null;
        }

        private void OnStatsChanged(StatsChangedEventArgs args)
        {
            ClearUI();
            DisplayNewUI();
        }

        private void ClearUI()
        {
            foreach (var data in Datas)
            {
                m_DestroyAction(data.Value);
            }

            Datas.Clear();
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_Source.CurrentStats.Stats)
            {
                UpsertUIData(item.Value);
            }
        }

        private void UpsertUIData(Stat stat)
        {
            if (!Datas.TryGetValue(stat.Name, out IStatUIData data))
            {
                data = m_InstantiationFunc();

                data.gameObject.SetActive(false);

                data.SetData(stat);

                data.gameObject.SetActive(true);

                Datas.Add(stat.Name, data);
            }
            else
            {
                data.SetData(stat);
            }
        }
    }
}