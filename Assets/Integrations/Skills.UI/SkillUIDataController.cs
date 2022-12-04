using System;
using Features.Skills;
using TMPro;
using UnityEngine;

namespace Integrations.Skills.UI
{
    public class SkillUIDataController : MonoBehaviour
    {
        private TextMeshProUGUI Title;
        
        private void Awake()
        {
            Title = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(SkillMetadata metadata)
        {
            name = metadata.InternalName;
            
            Title.text = metadata.DisplayName;
        }
    }
}