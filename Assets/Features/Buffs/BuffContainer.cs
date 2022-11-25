using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Buffs
{
    public class BuffContainer
    {
        private ConcurrentDictionary<string, ActiveBuff> m_Buffs;

        public Action<ActiveBuff> OnBuffAdded;
        public Action<ActiveBuff> OnBuffDurationReset;
        public Action<ActiveBuff> OnBuffRemoved;
        public Action<ActiveBuff> OnBuffStackAdded;
        public Action<ActiveBuff> OnBuffStackRemoved;
        public Action<ActiveBuff> OnBuffTickOccurred;

        public BuffContainer()
        {
            m_Buffs = new ConcurrentDictionary<string, ActiveBuff>();
        }

        public IReadOnlyList<ActiveBuff> Buffs => m_Buffs.Values.ToList();

        public void Tick(float deltaTime)
        {
            foreach (var activeBuff in m_Buffs)
            {
                activeBuff.Value.Tick(deltaTime);
            }

            foreach (var depletedBuff in m_Buffs.Where(x => x.Value.IsDepleted).ToList())
            {
                if (m_Buffs.TryRemove(depletedBuff.Key, out var removedBuff))
                {
                    OnBuffRemoved?.Invoke(removedBuff);
                }
            }
        }

        public void Receive(BuffMetadata buff, int stacks = 1)
        {
            Receive(buff, null, stacks);
        }

        public void Receive(BuffMetadata buff, GameObject source, int stacks = 1, float overrideDuration = -1f)
        {
            var existingBuff = BuffByName(buff.Name);

            if (existingBuff == null)
            {
                existingBuff = new ActiveBuff(buff, source, overrideDuration);

                existingBuff.AddStacks(stacks);

                existingBuff.RegisterCallbacks(OnBuffStackRemoved, OnBuffStackAdded, OnBuffTickOccurred,
                    OnBuffDurationReset);

                m_Buffs.TryAdd(buff.Name, existingBuff);

                OnBuffAdded?.Invoke(existingBuff);
            }
            else
            {
                if (Math.Abs(existingBuff.Duration - overrideDuration) > 0.0001f)
                {
                    existingBuff.OverrideDuration = overrideDuration;
                }

                existingBuff.AddStacks(stacks);
            }
        }

        public void Remove(BuffMetadata buff, int stacks = 1)
        {
            var existingBuff = BuffByName(buff.Name);

            existingBuff?.RemoveStacks(stacks);
        }

        private ActiveBuff BuffByName(string buff)
        {
            m_Buffs.FirstOrDefault(x => x.Value.Metadata.Name == buff).Deconstruct(out _, out var result);

            return result;
        }
    }
}