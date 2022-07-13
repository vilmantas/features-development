using Features.Buffs;
using Features.Equipment;
using Features.Health;
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

        hpController.OnDamageReceived.AddListener(c =>
            HPText.text = $"{c.After.ToString()}/{hpController.MaxHealth.ToString()}");
        hpController.OnHealingReceived.AddListener(c =>
            HPText.text = $"{c.After.ToString()}/{hpController.MaxHealth.ToString()}");

        buffController.WithUI(BuffPrefab, BuffContainer.transform);

        inventoryController.WithUI(InventoryPrefab, InventoryContainer.transform);

        equipmentController.WithUI(EquipmentPrefab, EquipmentContainer.transform);
    }
}