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

        private Action<ActiveBuff> m_OnBuffAdded;
        private Action<ActiveBuff> m_OnBuffRemoved;
        private Action<ActiveBuff> m_OnBuffStackAdded;
        private Action<ActiveBuff> m_OnBuffStackRemoved;
        private Action<ActiveBuff> m_OnBuffTickOccurred;

        public BuffContainer()
        {
            m_Buffs = new ConcurrentDictionary<string, ActiveBuff>();
        }

        public IReadOnlyList<ActiveBuff> Buffs => m_Buffs.Values.ToList();

        public BuffContainer RegisterCallbacks(
            Action<ActiveBuff> onBuffRemoved,
            Action<ActiveBuff> onBuffAdded,
            Action<ActiveBuff> onBuffStackRemoved,
            Action<ActiveBuff> onBuffStackAdded,
            Action<ActiveBuff> onBuffTickOccurred
        )
        {
            m_OnBuffAdded = onBuffAdded;
            m_OnBuffRemoved = onBuffRemoved;

            m_OnBuffStackAdded = onBuffStackAdded;
            m_OnBuffStackRemoved = onBuffStackRemoved;

            m_OnBuffTickOccurred = onBuffTickOccurred;

            return this;
        }

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
                    m_OnBuffRemoved?.Invoke(removedBuff);
                }
            }
        }

        public void Receive(BuffBase buff, int stacks = 1)
        {
            Receive(buff, null, stacks);
        }

        public void Receive(BuffBase buff, GameObject source, int stacks = 1)
        {
            var existingBuff = BuffByName(buff.Name);

            if (existingBuff == null)
            {
                existingBuff = new ActiveBuff(buff, source);

                existingBuff.AddStacks(stacks);

                existingBuff.RegisterCallbacks(m_OnBuffStackRemoved, m_OnBuffStackAdded, m_OnBuffTickOccurred);

                m_Buffs.TryAdd(buff.Name, existingBuff);

                m_OnBuffAdded?.Invoke(existingBuff);
            }
            else
            {
                existingBuff.AddStacks(stacks);
            }
        }

        public void Remove(BuffBase buff, int stacks = 1)
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