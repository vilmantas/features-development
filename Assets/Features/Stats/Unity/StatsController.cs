using UnityEngine;

namespace Stats.Unity
{
    public class StatsController : MonoBehaviour
    {
        [SerializeField] private Preset StartingStats;

        [HideInInspector] public StatsChangedEvent OnStatsChanged = new();

        private Manager Manager;
        public StatGroup CurrentStats => Manager.Current;

        public void Awake()
        {
            var startingStats = StartingStats == null ? null : StartingStats.Stats();

            Manager = new Manager(startingStats);
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