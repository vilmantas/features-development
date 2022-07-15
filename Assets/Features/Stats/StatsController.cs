using System;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Stats.Base
{
    public class StatsController : MonoBehaviour
    {
        [HideInInspector] public Action<StatsChangedEventArgs> OnStatsChanged;

        private Manager Manager;
        public StatGroup CurrentStats => Manager.Current;

        public void Awake()
        {
            Manager = new Manager(Array.Empty<Stat>());
        }

        public void WithUI(IStatUIData prefab, Transform container)
        {
            new StatsUIManager().SetSource(this,
                () =>
                {
                    var instance = Instantiate(prefab.gameObject, container);
                    return instance.GetComponentInChildren<IStatUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
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