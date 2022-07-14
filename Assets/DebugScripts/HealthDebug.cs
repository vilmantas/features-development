using Features.Health;
using Features.Health.Events;
using TMPro;
using UnityEngine;

public class HealthDebug : MonoBehaviour
{
    public HealthController HealthController;

    public TextMeshProUGUI Text;

    public TextMeshProUGUI Text2;

    private void Start()
    {
        Text.text = $"{HealthController.CurrentHealth}/{HealthController.MaxHealth}";

        HealthController.OnDamage += OnHealthChanged;

        HealthController.OnHeal += OnHealthChanged;
    }

    private void OnHealthChanged(HealthChangeEventArgs args)
    {
        Text.text = $"{args.After}/{args.Source.MaxHealth}";
        Text2.text = $"+{args.ActualChange}";
    }
}