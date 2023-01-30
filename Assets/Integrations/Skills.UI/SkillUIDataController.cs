using System;
using Features.Cooldowns;
using Features.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.RadialTimer;

namespace Integrations.Skills.UI
{
    public class SkillUIDataController : MonoBehaviour
    {
        public SkillMetadata Parent { get; private set; }
        
        private TextMeshProUGUI Title;
        
        private TextMeshProUGUI Index;

        private RadialTimerController Timer;

        private ActiveCooldown Cooldown;

        private Image Blocker;
        
        private void Awake()
        {
            Title = transform.Find("title").gameObject.GetComponent<TextMeshProUGUI>();
            
            Index = transform.Find("index").gameObject.GetComponent<TextMeshProUGUI>();
            
            Timer = GetComponentInChildren<RadialTimerController>();

            Blocker = transform.Find("blocker").gameObject.GetComponent<Image>();
        }

        private void Update()
        {
            if (Cooldown == null) return;
            
            Timer.SetFillAmount(Cooldown.StartCooldown, Cooldown.TimeLeft);
        }

        public void Initialize(SkillMetadata metadata, int index)
        {
            Parent = metadata;
            
            name = metadata.ReferenceName;
            
            Title.text = metadata.DisplayName;

            Index.text = index.ToString();
        }

        public void SetCooldown(ActiveCooldown cooldown)
        {
            Cooldown = cooldown;
        }

        public void SetBlock(bool status)
        {
            Blocker.gameObject.SetActive(status);
        }
    }
}