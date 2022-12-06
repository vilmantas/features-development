using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace UnityEngine
{
    public class ChannelingController : MonoBehaviour
    {
        public IReadOnlyDictionary<string, (ChannelingItem, ProgressReporter)> CurrentlyChanneling => m_CurrentlyChanneling;

        private Dictionary<string, (ChannelingItem, ProgressReporter)> m_CurrentlyChanneling = new();

        private GameObject Root;

        private Transform m_HeadAttachmentSpot;

        public ProgressReporter TimerPrefab;

        private void Update()
        {
            UpdateTimers(Time.deltaTime);
            
            RemoveExpired();
        }

        private void Awake()
        {
            Root = transform.root.gameObject;

            TimerPrefab = Resources.Load<ProgressReporter>("Prefabs/ChannelTimer");
        }

        private void Start()
        {
            foreach (Transform VARIABLE in Root.GetComponentsInChildren<Transform>())
            {
                if (VARIABLE.name != "Attachment_Head") continue;

                m_HeadAttachmentSpot = VARIABLE;
                
                break;
            }
        }

        public void StartChanneling(string title, float duration)
        {
            if (m_CurrentlyChanneling.ContainsKey(title)) return;
            
            var timer = Instantiate(TimerPrefab, m_HeadAttachmentSpot);
            
            timer.transform.position += new Vector3(0, 0.5f * m_CurrentlyChanneling.Count, 0);
            
            m_CurrentlyChanneling.Add(title, (new ChannelingItem(duration), timer));
        }

        public void ContinueChanneling(string title, float current, float max)
        {
            if (m_CurrentlyChanneling.ContainsKey(title)) return;
            
            var timer = Instantiate(TimerPrefab, m_HeadAttachmentSpot);
            
            timer.transform.position += new Vector3(0, 0.5f * m_CurrentlyChanneling.Count, 0);
            
            m_CurrentlyChanneling.Add(title, (new ChannelingItem(max, current), timer));
        }

        private void UpdateTimers(float timeDelta)
        {
            foreach (var valueTuple in m_CurrentlyChanneling)
            {
                var channelThing = valueTuple.Value.Item1;
                
                channelThing.AddToChannel(timeDelta);

                var timer = valueTuple.Value.Item2;

                 timer.SetText(
                    $"{channelThing.ChanneledAmount:#.##}/{channelThing.MaxDuration:#.##}");
            }
        }
        
        private void RemoveExpired()
        {
            var expiredCooldowns = m_CurrentlyChanneling.Where(x => x.Value.Item1.IsCompleted);

            m_CurrentlyChanneling = m_CurrentlyChanneling.Where(x => !x.Value.Item1.IsCompleted)
                .ToDictionary(x => x.Key, x => x.Value);
            
            foreach (var keyValuePair in expiredCooldowns)
            {
                Destroy(keyValuePair.Value.Item2.gameObject);
            }
        }
    }
}