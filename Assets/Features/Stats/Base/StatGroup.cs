using System;
using System.Collections.Generic;
using System.Linq;

namespace Stats
{
    [Serializable]
    public class StatGroup
    {
        public readonly Guid Id = Guid.NewGuid();

        private readonly Dictionary<string, Stat> m_Stats;

        public StatGroup(Dictionary<string, Stat> stats)
        {
            m_Stats = stats;
        }

        public StatGroup(Stat[] stats)
        {
            m_Stats = new();

            if (stats == null) return;

            m_Stats = stats.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => new Stat(x.Key, x.Sum(x => x.Value)));
        }

        public IReadOnlyDictionary<string, Stat> Stats => m_Stats;

        public StatGroup Difference(StatGroup other)
        {
            var resultGroup = EmptyStats();

            foreach (var stat in m_Stats)
            {
                if (other.m_Stats.TryGetValue(stat.Key, out var otherStat))
                {
                    resultGroup[stat.Key] = m_Stats[stat.Key].Combine(-otherStat.Value);
                }
            }

            resultGroup = resultGroup.Where(x => x.Value.Value != 0).ToDictionary(x => x.Key, x => x.Value);

            return new StatGroup(resultGroup);
        }

        public StatGroup Combine(StatGroup other)
        {
            var result = StatsCopy();

            foreach (var otherStat in other.Stats)
            {
                if (Stats.TryGetValue(otherStat.Key, out var stat))
                {
                    result[otherStat.Key] = stat.Combine(otherStat.Value);
                }
                else
                {
                    result.Add(otherStat.Key, new Stat(otherStat.Key, otherStat.Value.Value));
                }
            }

            return new StatGroup(result);
        }

        private Dictionary<string, Stat> EmptyStats() =>
            m_Stats.ToDictionary(x => x.Key, x => new Stat(x.Value.Name, 0));

        private Dictionary<string, Stat> StatsCopy() => m_Stats.ToDictionary(x => x.Key, x => x.Value.Copy);

        public StatGroup AsEmptyGroup() => new StatGroup(EmptyStats());
    }
}