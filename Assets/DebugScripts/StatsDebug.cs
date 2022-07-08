using System.Collections.Generic;
using Stats;
using Stats.Unity;
using UnityEngine;

namespace DebugScripts
{
    public class StatsDebug : MonoBehaviour
    {
        private StatsController m_StatsController;

        private void Start()
        {
            m_StatsController = GetComponentInChildren<StatsController>();

            m_StatsController.OnStatsChanged.AddListener(LogStats);
        }

        private void LogStats(StatsChangedEventArgs arg0)
        {
            foreach (var newStat in arg0.New.Stats)
            {
                Debug.Log($"{newStat.Key} - {newStat.Value.Value}");
            }
        }

        public void AddStat()
        {
            m_StatsController.ApplyStatModifiers(new StatGroup(new List<Stat>()
            {
                new("Strength", 1),
                new("Defence", 1),
                new("Wisdom", 1),
            }.ToArray()));
        }
    }
}