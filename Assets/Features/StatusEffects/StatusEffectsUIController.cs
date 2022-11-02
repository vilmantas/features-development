using System.Linq;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Features.Conditions
{
    public class StatusEffectsUIController : MonoBehaviour
    {
        private StatusEffectsController m_source;

        public TextMeshProUGUI Text;
        
        public void Initialize(StatusEffectsController source)
        {
            m_source = source;
            
            m_source.OnAdded += UpdateUI;

            m_source.OnRemoved += UpdateUI;
        }

        private void UpdateUI(StatusEffectMetadata obj)
        {
            var conditions = m_source.StatusEffects.Select(x => x.Metadata.DisplayName);
            
            Text.text = string.Join(", ", conditions);
        }
    }
}