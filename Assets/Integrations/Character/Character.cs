using System;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character
{
    public class CharacterC
    {
        [Serializable]
        public class StartingEquipmentItem
        {
            public Item_SO Item;
            public string Slot;
        }

        [RequireComponent(typeof(CharacterEvents))]
        public class Character : MonoBehaviour
        {
            public bool Buffs;

            public bool Inventory;

            public bool Equipment;

            public bool Stats;

            public bool Health;

            [Range(1, 100)] public int MaxHealth = 20;

            [Range(1, 100)] public int CurrentHealth = 10;

            [Range(1, 100)] public int InventorySize = 5;

            public SlotData[] EquipmentSlots = new[] {new SlotData() {slotType = "Main"}};

            public Item_SO[] StartingInventory;

            public StartingEquipmentItem[] StartingEquipment;

            public Buff_SO[] StartingBuffs;

            public Stats_SO BaseStats;

            [HideInInspector] public BuffController m_BuffController;

            [HideInInspector] public EquipmentController m_EquipmentController;

            [HideInInspector] public InventoryController m_InventoryController;

            [HideInInspector] public StatsController m_StatsController;

            [HideInInspector] public ActionsController m_ActionsController;

            private CharacterActionsManager m_ActionsManager;

            private CharacterBuffsManager m_BuffsManager;

            private CharacterEquipmentManager m_EquipmentManager;

            private HealthController m_HealthController;

            private CharacterInventoryManager m_InventoryManager;

            private CharacterStatsManager m_StatsManager;

            private void Awake()
            {
                var root = transform;

                AddSystems(root);

                AddManagers(root);

                if (Inventory && StartingInventory != null && StartingInventory.Length > 0)
                {
                    foreach (var itemSo in StartingInventory)
                    {
                        if (!itemSo) continue;

                        var itemInstance = itemSo.MakeInstanceWithCount();

                        m_InventoryController.HandleRequest(ChangeRequestFactory.Add(itemInstance.StorageData));
                    }
                }

                if (Buffs && StartingBuffs != null && StartingBuffs.Length > 0)
                {
                    foreach (var startingBuff in StartingBuffs)
                    {
                        if (!startingBuff) continue;

                        m_BuffController.AttemptAdd(new(startingBuff.Base, gameObject, 1));
                    }
                }

                if (Equipment && StartingEquipment != null && StartingEquipment.Length > 0)
                {
                    foreach (var equipmentItem in StartingEquipment)
                    {
                        if (equipmentItem == null || equipmentItem.Item == null) continue;

                        var instance = equipmentItem.Item.MakeInstanceWithCount();

                        var request = new EquipRequest() {Item = instance, Slot = equipmentItem.Slot};

                        m_EquipmentController.HandleEquipRequest(request);
                    }
                }
            }

            private void AddManagers(Transform root)
            {
                var managersParent = new GameObject("managers").transform;

                managersParent.parent = root;

                AddComponent(managersParent, "actions", ref m_ActionsManager);

                if (Inventory)
                {
                    AddComponent(managersParent, "inventory", ref m_InventoryManager);
                }

                if (Equipment)
                {
                    AddComponent(managersParent, "equipment", ref m_EquipmentManager);
                }

                if (Buffs)
                {
                    AddComponent(managersParent, "buffs", ref m_BuffsManager);
                }

                if (Stats)
                {
                    AddComponent(managersParent, "stats", ref m_StatsManager);
                }
            }

            private void AddSystems(Transform root)
            {
                var systemsParent = new GameObject("systems").transform;

                systemsParent.parent = root;

                AddComponent(systemsParent, "actions", ref m_ActionsController);

                if (Inventory)
                {
                    AddComponent(systemsParent, "inventory", ref m_InventoryController);

                    m_InventoryController.Initialize(InventorySize);
                }

                if (Equipment)
                {
                    AddComponent(systemsParent, "equipment", ref m_EquipmentController);

                    m_EquipmentController.Initialize(EquipmentSlots);
                }

                if (Buffs)
                {
                    AddComponent(systemsParent, "buffs", ref m_BuffController);
                }

                if (Stats)
                {
                    AddComponent(systemsParent, "stats", ref m_StatsController);

                    if (BaseStats)
                    {
                        m_StatsController.Initialize(BaseStats.Stats);
                    }
                }

                if (Health)
                {
                    AddComponent(systemsParent, "health", ref m_HealthController);

                    m_HealthController.Initialize(CurrentHealth, MaxHealth);
                }
            }

            private void AddComponent<T>(Transform parent, string componentName, ref T holder) where T : MonoBehaviour
            {
                var i = new GameObject(componentName);

                i.transform.parent = parent;

                holder = i.AddComponent<T>();
            }
        }
    }
}