using Features.Buffs;
using Features.Equipment;
using Features.Inventory;
using Inventory.Unity;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Character;

    public GameObject BuffContainer;

    public GameObject InventoryContainer;

    public GameObject EquipmentContainer;

    public BaseBuffUIData BuffPrefab;

    public BaseInventoryUIData InventoryPrefab;

    public BaseEquipmentUIData EquipmentPrefab;

    private void Start()
    {
        var buffController = Character.GetComponentInChildren<BuffController>();

        var inventoryController = Character.GetComponentInChildren<InventoryController>();

        var equipmentController = Character.GetComponentInChildren<EquipmentController>();

        buffController.WithUI(BuffPrefab, BuffContainer.transform);

        inventoryController.WithUI(InventoryPrefab, InventoryContainer.transform);

        equipmentController.WithUI(EquipmentPrefab, EquipmentContainer.transform);
    }
}