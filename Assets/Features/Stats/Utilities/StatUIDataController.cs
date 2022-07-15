using Features.Stats.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Stats.Base
{
    [ExecuteAlways]
    public class StatUIDataController : BaseStatUIData
    {
        [HideInInspector] public TextMeshProUGUI Title;

        [HideInInspector] public TextMeshProUGUI Value;

        [HideInInspector] public Image Background;

        public override void OnAwake()
        {
            var children = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var textMeshProUGUI in children)
            {
                if (textMeshProUGUI.name.EndsWith("title"))
                {
                    Title = textMeshProUGUI;
                }

                if (textMeshProUGUI.name.EndsWith("value"))
                {
                    Value = textMeshProUGUI;
                }
            }

            Background = GetComponentInChildren<Image>();
        }

        public override void OnSetData(Stat stat)
        {
            Title.text = stat.Name;
            Value.text = stat.Value.ToString();
        }
    }
}