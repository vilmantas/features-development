using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ContextMenuOptionUI : MonoBehaviour
{
    public ContextOptionSelected ContextOptionSelected;

    private Button Button;
    private TextMeshProUGUI OptionText;

    private void Awake()
    {
        Button = GetComponentInChildren<Button>();

        Button.onClick.AddListener(OnButtonPress);

        var children = GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var textMeshProUGUI in children)
        {
            if (textMeshProUGUI.name.EndsWith("text"))
            {
                OptionText = textMeshProUGUI;
            }
        }
    }

    private void OnDestroy()
    {
        ContextOptionSelected.RemoveAllListeners();
    }

    private void OnButtonPress()
    {
        ContextOptionSelected.Invoke(OptionText.text);
    }

    public void SetData(string option)
    {
        OptionText.text = option;
    }
}

[Serializable]
public class ContextOptionSelected : UnityEvent<string>
{
}