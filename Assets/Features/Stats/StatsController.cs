using System;
using UnityEngine;

namespace Features.Stats.Base
{
    public class StatsController : MonoBehaviour
    {
        private Manager Manager;

        public Action<StatsChangedEventArgs> OnStatsChanged;

        public StatGroup CurrentStats => Manager.Current;

        public void Awake()
        {
            Manager = new Manager(Array.Empty<Stat>());
        }

        public void Initialize(Stat[] stats)
        {
            Manager = new Manager(stats);
        }

        public void ApplyStatModifiers(StatGroup request)
        {
            var previousStats = Manager.Current;

            var newStats = Manager.ApplyModifiers(request);

            OnStatsChanged?.Invoke(new StatsChangedEventArgs(previousStats, newStats));
        }

        public void RemoveStatModifier(StatGroup request)
        {
            var previousStats = Manager.Current;

            var newStats = Manager.RemoveModifier(request);

            OnStatsChanged?.Invoke(new StatsChangedEventArgs(previousStats, newStats));
        }
    }
}