using System.Collections.Generic;
using Features.Stats.Base;
using UnityEngine;

namespace DebugScripts
{
    public class StatsDebug : MonoBehaviour
    {
        public BaseStatUIData UIPrefab;

        public Transform UIContainer;
        private StatsController m_StatsController;

        private void Start()
        {
            m_StatsController = GetComponentInChildren<StatsController>();

            var statsUIManager = new StatsUIManager();

            statsUIManager.SetSource(m_StatsController,
                () =>
                {
                    var instance = Instantiate(UIPrefab.gameObject, UIContainer.transform);
                    return instance.GetComponentInChildren<IStatUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
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