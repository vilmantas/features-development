using System;
using Features.Cooldowns;
using Features.Skills;
using TMPro;
using UnityEngine;
using Utilities.RadialTimer;

namespace Integrations.Skills.UI
{
    public class SkillUIDataController : MonoBehaviour
    {
        public SkillMetadata Parent { get; private set; }
        
        private TextMeshProUGUI Title;

        private RadialTimerController Timer;

        private ActiveCooldown Cooldown;
        
        private void Awake()
        {
            Title = GetComponentInChildren<TextMeshProUGUI>();

            Timer = GetComponentInChildren<RadialTimerController>();
        }

        private void Update()
        {
            if (Cooldown == null) return;
            
            Timer.SetFillAmount(Cooldown.StartCooldown, Cooldown.TimeLeft);
        }

        public void Initialize(SkillMetadata metadata)
        {
            Parent = metadata;
            
            name = metadata.ReferenceName;
            
            Title.text = metadata.DisplayName;
        }

        public void SetCooldown(ActiveCooldown cooldown)
        {
            Cooldown = cooldown;
        }
    }
}