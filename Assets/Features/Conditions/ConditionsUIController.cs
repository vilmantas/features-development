using System.Linq;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Features.Conditions
{
    public class ConditionsUIController : MonoBehaviour
    {
        private ConditionsController m_source;

        public TextMeshProUGUI Text;
        
        public void Initialize(ConditionsController source)
        {
            m_source = source;
            
            m_source.OnConditionAdded += OnConditionAdded;
        }

        private void OnConditionAdded(StatusCondition obj)
        {
            var conditions = m_source.Conditions.Select(x => x.DisplayName);
            
            Text.text = string.Join(", ", conditions);
        }
    }
}