using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace UnityEngine
{
    public class ChannelingController : MonoBehaviour
    {
        public IReadOnlyList<ChannelingItem> CurrentlyChanneling => m_CurrentlyChanneling;

        private List<ChannelingItem> m_CurrentlyChanneling = new();

        public Action<ChannelingItem> OnChannelingStarted;

        public Action<float> OnChannelingTick;

        public Action<ChannelingItem> OnChannelingCompleted;

        private void Update()
        {
            var delta = Time.deltaTime;
            
            ProgressTimers(delta);
            
            OnChannelingTick?.Invoke(delta);
            
            RemoveExpired();
        }
        
        private void ProgressTimers(float timeDelta)
        {
            foreach (var channelingItem in m_CurrentlyChanneling)
            {
                channelingItem.AddProgress(timeDelta);
            }
        }

        public void StartChanneling(ChannelingCommand command)
        {
            if (m_CurrentlyChanneling.Any(x => x.Title.Equals(command.Title))) return;

            var tracker = new ChannelingItem(command.Title, command.Max, command.Current);

            tracker.Callback = command.Callback;
            
            m_CurrentlyChanneling.Add(tracker);

            OnChannelingStarted?.Invoke(tracker);
        }
        
        private void RemoveExpired()
        {
            var expiredCooldowns = m_CurrentlyChanneling.Where(x => x.IsCompleted);

            m_CurrentlyChanneling = m_CurrentlyChanneling.Where(x => !x.IsCompleted).ToList();
            
            foreach (var channelingItem in expiredCooldowns)
            {
                OnChannelingCompleted?.Invoke(channelingItem);
                
                channelingItem.Callback?.Invoke();
            }
        }
    }
}