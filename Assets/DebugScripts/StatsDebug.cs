using System.Collections.Generic;
using Features.Stats.Base;
using UnityEngine;

namespace DebugScripts
{
    public class StatsDebug : MonoBehaviour
    {
        private StatsController m_StatsController;

        public BaseStatUIData UIPrefab;

        public Transform UIContainer;

        private void Start()
        {
            m_StatsController = GetComponentInChildren<StatsController>();

            m_StatsController.WithUI(UIPrefab, UIContainer);
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