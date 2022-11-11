using System;
using Features.Actions;
using Features.Buffs;
using Features.Combat;
using Features.Conditions;
using Features.Equipment;
using Features.Health;
using Features.Inventory;
using Features.Movement;
using Features.Stats.Base;
using Integrations.Items;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Character
{
    public class Modules
    {
        [Serializable]
        public class StartingEquipmentItem
        {
            public Item_SO Item;
            public string Slot;
        }

        [RequireComponent(typeof(CharacterEvents))]
        [RequireComponent(typeof(Rigidbody))]
        public class Character : MonoBehaviour
        {
            [HideInInspector] public CharacterEvents Events;

            [HideInInspector] public Rigidbody Rigidbody;
            
            public bool Buffs;

            public bool Inventory;

            public bool Equipment;

            public bool Stats;

            public bool Health;

            public bool Combat;

            public bool Conditions;
            
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

            [HideInInspector] public CombatController m_CombatController;

            [HideInInspector] public StatusEffectsController m_StatusEffectsController;

            [HideInInspector] public MovementController m_MovementController;

            [HideInInspector] public CharacterStatCalculator m_CharacterStatCalculator;

            [HideInInspector] public CharacterItemManager m_CharacterItemManager;

            private CharacterActionsManager m_ActionsManager;

            private CharacterBuffsManager m_BuffsManager;

            private CharacterEquipmentManager m_EquipmentManager;

            [HideInInspector] public HealthController m_HealthController;

            private CharacterInventoryManager m_InventoryManager;

            private CharacterStatsManager m_StatsManager;

            private CharacterCombatManager m_CombatManager;

            private CharacterStatusEffectsManager m_CharacterStatusEffectsManager;

            private void AddManagers(Transform root)
            {
                var managersParent = new GameObject("managers").transform;

                managersParent.parent = root;
                
                AddComponent(managersParent, "actions", ref m_ActionsManager);
                AddComponent(managersParent, "stat_calculator", ref m_CharacterStatCalculator);
                AddComponent(managersParent, "item_manager", ref m_CharacterItemManager);
                

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

                if (Combat)
                {
                    AddComponent(managersParent, "combat", ref m_CombatManager);
                }

                if (Conditions)
                {
                    AddComponent(managersParent, "status_effects", ref m_CharacterStatusEffectsManager);
                }
            }

            private void Awake()
            {
                var root = transform;

                Events = GetComponent<CharacterEvents>();

                Rigidbody = GetComponent<Rigidbody>();

                Rigidbody.isKinematic = true;

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

            private void AddSystems(Transform root)
            {
                var systemsParent = new GameObject("systems").transform;

                systemsParent.parent = root;

                AddComponent(systemsParent, "actions", ref m_ActionsController);
                
                AddComponent(systemsParent, "movement", ref m_MovementController);

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

                if (Combat)
                {
                    AddComponent(systemsParent, "combat", ref m_CombatController);
                }

                if (Conditions)
                {
                    AddComponent(systemsParent, "conditions", ref m_StatusEffectsController);
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