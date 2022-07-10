using System;
using UnityEngine;

namespace Stats.Unity
{
    public class StatController : MonoBehaviour
    {
        [HideInInspector] public StatsChangedEvent OnStatsChanged = new();

        private Manager Manager;
        public StatGroup CurrentStats => Manager.Current;

        public void Awake()
        {
            Manager = new Manager(Array.Empty<Stat>());
        }

        public void ApplyStatModifiers(StatGroup request)
        {
            var previousStats = Manager.Current;

            var newStats = Manager.ApplyModifiers(request);

            OnStatsChanged.Invoke(new StatsChangedEventArgs(previousStats, newStats));
        }

        public void RemoveStatModifier(StatGroup request)
        {
            var previousStats = Manager.Current;

            var newStats = Manager.RemoveModifier(request);

            OnStatsChanged.Invoke(new StatsChangedEventArgs(previousStats, newStats));
        }
    }
}