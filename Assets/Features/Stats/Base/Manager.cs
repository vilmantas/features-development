using System.Collections.Generic;

namespace Features.Stats.Base
{
    internal class Manager
    {
        private readonly List<StatGroup> AppliedModifiers = new();

        private readonly StatGroup BaseStats;

        private StatGroup CurrentStats;
        private int Level;

        public Manager(Stat[] baseStats)
        {
            BaseStats = new StatGroup(baseStats);

            CurrentStats = new StatGroup(baseStats);
        }

        public StatGroup Current => CurrentStats;

        public StatGroup ApplyModifiers(StatGroup request)
        {
            AppliedModifiers.Add(request);

            CalculateNewStats();

            return Current;
        }

        public StatGroup RemoveModifier(StatGroup request)
        {
            AppliedModifiers.Remove(request);

            CalculateNewStats();

            return Current;
        }

        private void CalculateNewStats()
        {
            var result = BaseStats.AsEmptyGroup();

            result = result.Combine(BaseStats);

            foreach (var modifier in AppliedModifiers)
            {
                result = result.Combine(modifier);
            }

            CurrentStats = result;
        }
    }
}