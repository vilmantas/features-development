using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEngine
{
    public class ChannelingUIController : MonoBehaviour
    {
        private readonly Dictionary<string, ChannelingItemUI> m_CurrentlyChanneling = new();
        
        private ChannelingController m_Source;

        private ChannelingItemUI m_TimerPrefab;
        
        private Transform m_HeadAttachmentSpot;
        
        private void Awake()
        {
            m_TimerPrefab = Resources.Load<ChannelingItemUI>("Prefabs/ChannelTimer");
            
            foreach (Transform VARIABLE in transform.root.GetComponentsInChildren<Transform>())
            {
                if (VARIABLE.name != "Attachment_Head") continue;

                m_HeadAttachmentSpot = VARIABLE;
                
                break;
            }
        }

        public void Initialize(ChannelingController source)
        {
            m_Source = source;
            
            m_Source.OnChannelingStarted += OnChannelingStarted;
            
            m_Source.OnChannelingTick += OnChannelingTick;
            
            m_Source.OnChannelingCompleted += OnChannelingCompleted;
        }

        private void OnChannelingCompleted(ChannelingItem obj)
        {
            if (!m_CurrentlyChanneling.TryGetValue(obj.Title, out var item)) return;
            
            Destroy(item.gameObject);
        }

        private void OnChannelingTick(float obj)
        {
            foreach (var channelingItemUI in m_CurrentlyChanneling)
            {
                var item = m_Source.CurrentlyChanneling.FirstOrDefault(x =>
                    x.Title.Equals(channelingItemUI.Key));

                if (item == null) continue;

                channelingItemUI.Value.SetFillAmount(item.ChanneledAmount, item.MaxDuration);
            }
        }

        private void OnChannelingStarted(ChannelingItem obj)
        {
            var timer = Instantiate(m_TimerPrefab, m_HeadAttachmentSpot);

            timer.transform.position +=
                new Vector3(0, 0.5f * m_Source.CurrentlyChanneling.Count, 0);
            
            m_CurrentlyChanneling.Add(obj.Title, timer);
        }
    }
}