using Features.Buffs;
using Features.Equipment;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Inventory.Unity;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Character;

    public GameObject BuffContainer;

    public GameObject InventoryContainer;

    public GameObject EquipmentContainer;

    public TextMeshProUGUI HPText;

    public BaseBuffUIData BuffPrefab;

    public BaseInventoryUIData InventoryPrefab;

    public BaseEquipmentUIData EquipmentPrefab;


    private void Start()
    {
        var buffController = Character.GetComponentInChildren<BuffController>();

        var inventoryController = Character.GetComponentInChildren<InventoryController>();

        var equipmentController = Character.GetComponentInChildren<EquipmentController>();

        var hpController = Character.GetComponentInChildren<HealthController>();

        HPText.text = $"{hpController.CurrentHealth.ToString()}/{hpController.MaxHealth.ToString()}";

        hpController.OnDamageReceived += OnHealthChanged;
        hpController.OnHealingReceived += OnHealthChanged;

        buffController.WithUI(BuffPrefab, BuffContainer.transform);

        inventoryController.WithUI(InventoryPrefab, InventoryContainer.transform);

        equipmentController.WithUI(EquipmentPrefab, EquipmentContainer.transform);
    }

    public void OnHealthChanged(HealthChangeEventArgs args)
    {
        HPText.text = $"{args.After.ToString()}/{args.Source.MaxHealth.ToString()}";
    }
}