using Features.Buffs;
using Features.Buffs.UI;
using Features.Equipment;
using Features.Equipment.UI;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Inventory.UI;
using Features.Stats.Base;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Character;

    public GameObject BuffContainer;

    public GameObject InventoryContainer;

    public GameObject EquipmentContainer;

    public GameObject StatsContainer;

    public TextMeshProUGUI HPText;

    public BaseBuffUIData BuffPrefab;

    public BaseInventoryUIData InventoryPrefab;

    public BaseEquipmentUIData EquipmentPrefab;

    public BaseStatUIData StatPrefab;

    private void Start()
    {
        var buffController = Character.GetComponentInChildren<BuffController>();

        var inventoryController = Character.GetComponentInChildren<InventoryController>();

        var equipmentController = Character.GetComponentInChildren<EquipmentController>();

        var hpController = Character.GetComponentInChildren<HealthController>();

        var statsController = Character.GetComponentInChildren<StatsController>();

        HPText.text = $"{hpController.CurrentHealth.ToString()}/{hpController.MaxHealth.ToString()}";

        hpController.OnDamage += OnHealthChanged;
        hpController.OnHeal += OnHealthChanged;

        var buffUIManager = new BuffUIManager();
        
        buffUIManager.SetSource(buffController,
            () =>
            {
                var instance = Instantiate(BuffPrefab.gameObject, BuffContainer.transform);
                return instance.GetComponentInChildren<IBuffUIData>();
            },
            controller => DestroyImmediate(controller.gameObject));

        var inventoryUIManager = new InventoryUIManager();
        
        inventoryUIManager.SetSource(inventoryController,
            () =>
            {
                var instance = Instantiate(InventoryPrefab.gameObject, InventoryContainer.transform);
                return instance.GetComponentInChildren<IInventoryUIData>();
            },
            controller => DestroyImmediate(controller.gameObject));

        var equipmentUIManager = new EquipmentUIManager();
        
        equipmentUIManager.SetSource(equipmentController,
            () =>
            {
                var instance = Instantiate(EquipmentPrefab.gameObject, EquipmentContainer.transform);
                return instance.GetComponentInChildren<IEquipmentUIData>();
            },
            controller => DestroyImmediate(controller.gameObject));

        statsController.WithUI(StatPrefab, StatsContainer.transform);
    }

    public void OnHealthChanged(HealthChangeEventArgs args)
    {
        HPText.text = $"{args.After.ToString()}/{args.Source.MaxHealth.ToString()}";
    }
}