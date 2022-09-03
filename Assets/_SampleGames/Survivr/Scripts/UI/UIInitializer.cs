using _SampleGames.Survivr;
using Features.Buffs;
using Features.Buffs.UI;
using Features.Inventory;
using Features.Inventory.UI;
using UnityEngine;

public class UIInitializer : Manager
{
    public InventoryUIController InventoryUI;

    public BuffUIController BuffUI;
    private GameObject m_Player;

    public override void Initialize()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");

        var inv = m_Player.GetComponentInChildren<InventoryController>();

        if (inv && InventoryUI) InventoryUI.Initialize(inv);

        var buffs = m_Player.GetComponentInChildren<BuffController>();

        if (inv && InventoryUI) BuffUI.Initialize(buffs);
    }
}