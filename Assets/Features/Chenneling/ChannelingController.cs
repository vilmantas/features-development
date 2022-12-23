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
            
            RemoveCompleted();
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

            var tracker = new ChannelingItem(command.Title, command.Max, command.Current, command.Data);

            tracker.Callback = command.Callback;
            
            m_CurrentlyChanneling.Add(tracker);

            OnChannelingStarted?.Invoke(tracker);
        }

        public void InterruptChanneling(string title)
        {
            var item = m_CurrentlyChanneling.FirstOrDefault(x => x.Title.Equals(title));
            
            item?.Interrupted();
        }
        
        private void RemoveCompleted()
        {
            var completedChannels = m_CurrentlyChanneling.Where(x => x.IsCompleted || x.IsInterrupted);

            m_CurrentlyChanneling = m_CurrentlyChanneling.Where(x => !x.IsCompleted && !x.IsInterrupted).ToList();
            
            foreach (var channelingItem in completedChannels)
            {
                OnChannelingCompleted?.Invoke(channelingItem);
                
                channelingItem.Callback?.Invoke();
            }
        }
    }
}