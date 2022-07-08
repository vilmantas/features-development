using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stats.Unity
{
    [ExecuteAlways]
    public class StatsUIData : MonoBehaviour
    {
        [HideInInspector] public TextMeshProUGUI Title;

        [HideInInspector] public TextMeshProUGUI Value;

        [HideInInspector] public Image Background;

        private void Awake()
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

        public void SetData(string stat, int value)
        {
            Title.text = stat;
            Value.text = value.ToString();
        }
    }
}