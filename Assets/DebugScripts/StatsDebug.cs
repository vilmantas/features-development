using System.Collections.Generic;
using UnityEngine;

namespace DebugScripts
{
    public class StatsDebug : MonoBehaviour
    {
        private StatController m_StatController;

        private void Start()
        {
            m_StatController = GetComponentInChildren<StatController>();

            m_StatController.OnStatsChanged.AddListener(LogStats);
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
            m_StatController.ApplyStatModifiers(new StatGroup(new List<Stat>()
            {
                new("Strength", 1),
                new("Defence", 1),
                new("Wisdom", 1),
            }.ToArray()));
        }
    }
}