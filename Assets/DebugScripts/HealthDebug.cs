using Features.Health;
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

        HealthController.OnDamageReceived.AddListener(result =>
        {
            Text.text = $"{result.After}/{HealthController.MaxHealth}";
            Text2.text = $"{result.ActualChange}";
        });

        HealthController.OnHealingReceived.AddListener(result =>
        {
            Text.text = $"{result.After}/{HealthController.MaxHealth}";
            Text2.text = $"+{result.ActualChange}";
        });
    }
}