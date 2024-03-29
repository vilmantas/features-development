using System;
using System.Linq;
using Features.Actions;
using Features.Buffs;
using Features.CharacterModel;
using Features.Combat;
using Features.Conditions;
using Features.Cooldowns;
using Features.Equipment;
using Features.Health;
using Features.Health.UI;
using Features.Inventory;
using Features.Movement;
using Features.OverheadParticles;
using Features.Skills;
using Features.Stats.Base;
using Features.Targeting;
using Features.WeaponAnimationConfigurations;
using Integrations.Items;
using UnityEngine;

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

            public bool Overheads;

            public bool Skills;

            public bool HealthOverhead;

            [Range(1, 100)] public int MaxHealth = 20;

            [Range(1, 100)] public int CurrentHealth = 10;

            [Range(1, 100)] public int InventorySize = 5;

            public SlotData[] EquipmentSlots = new[] {new SlotData() {slotType = "Main"}};

            public Item_SO[] StartingInventory;

            public StartingEquipmentItem[] StartingEquipment;

            public Buff_SO[] StartingBuffs;

            public Stats_SO BaseStats;

            public Skill_SO[] StartingSkills;

            [HideInInspector] public BuffController m_BuffController;

            [HideInInspector] public EquipmentController m_EquipmentController;

            [HideInInspector] public InventoryController m_InventoryController;

            [HideInInspector] public StatsController m_StatsController;

            [HideInInspector] public ActionsController m_ActionsController;

            [HideInInspector] public CombatController m_CombatController;

            [HideInInspector] public StatusEffectsController m_StatusEffectsController;

            [HideInInspector] public MovementController m_MovementController;

            [HideInInspector] public CharacterStatCalculator m_StatCalculator;

            [HideInInspector] public CharacterItemManager m_ItemManager;

            [HideInInspector] public CharacterOverheadsManager m_OverheadsManager;

            [HideInInspector] public OverheadsController m_OverheadsController;

            [HideInInspector] public SkillsController m_SkillsController;

            [HideInInspector] public CooldownsController m_CooldownsController;

            [HideInInspector] public ChannelingController m_ChannelingController;

            [HideInInspector] public HealthController m_HealthController;

            [HideInInspector] public HitboxAnimationController m_HitboxAnimationController;

            private CharacterActionsManager m_ActionsManager;

            private CharacterBuffsManager m_BuffsManager;

            private CharacterChannelingManager m_ChannelingManager;

            [HideInInspector] private ChannelingUIController m_ChannelingUIController;

            private CharacterCombatManager m_CombatManager;

            private CharacterCooldownsManager m_CooldownsManager;

            private CharacterEquipmentManager m_EquipmentManager;

            private CharacterExpirationManager m_ExpirationManager;

            private CharacterInventoryManager m_InventoryManager;

            private CharacterSkillsManager m_SkillsManager;

            private CharacterStatsManager m_StatsManager;

            private CharacterStatusEffectsManager m_StatusEffectsManager;

            private TargetProvider m_TargetProvider;
            
            private bool m_HealthInitialized;

            private void Awake()
            {
                var root = transform;

                Events = GetComponent<CharacterEvents>();

                Rigidbody = GetComponent<Rigidbody>();

                Rigidbody.isKinematic = true;

                AddSystems(root);

                AddManagers(root);

                PrepareCharacter();
            }

            private void AddSystems(Transform root)
            {
                var systemsParent = new GameObject("systems").transform;

                systemsParent.parent = root;
                
                systemsParent.transform.position = Vector3.zero;

                void AddSystemsComponent<T>(string componentName, ref T holder)
                    where T : MonoBehaviour =>
                    AddComponent(systemsParent, componentName, ref holder);

                AddSystemsComponent("actions", ref m_ActionsController);
                AddSystemsComponent("movement", ref m_MovementController);
                AddSystemsComponent("cooldowns", ref m_CooldownsManager);
                AddSystemsComponent("channeling", ref m_ChannelingController);
                AddSystemsComponent("channeling_ui", ref m_ChannelingUIController);
                AddSystemsComponent("target_provider", ref m_TargetProvider);
                AddSystemsComponent("hitbox_animation", ref m_HitboxAnimationController);

                m_ChannelingUIController.Initialize(m_ChannelingController);

                ConfigureTargetingSystem(m_TargetProvider);

                if (Inventory)
                {
                    AddSystemsComponent("inventory", ref m_InventoryController);
                }

                if (Equipment)
                {
                    AddSystemsComponent("equipment", ref m_EquipmentController);
                }

                if (Buffs)
                {
                    AddSystemsComponent("buffs", ref m_BuffController);
                }

                if (Stats)
                {
                    AddSystemsComponent("stats", ref m_StatsController);
                }

                if (Health)
                {
                    AddSystemsComponent("health", ref m_HealthController);
                }

                if (Combat)
                {
                    AddSystemsComponent("combat", ref m_CombatController);
                }

                if (Conditions)
                {
                    AddSystemsComponent("conditions", ref m_StatusEffectsController);
                }

                if (Overheads)
                {
                    AddSystemsComponent("overheads", ref m_OverheadsController);
                }

                if (Skills)
                {
                    AddSystemsComponent("skills", ref m_SkillsController);
                }
            }

            private void AddManagers(Transform root)
            {
                var managersParent = new GameObject("managers").transform;

                managersParent.parent = root;
                
                managersParent.transform.position = Vector3.zero;

                void AddManagerComponent<T>(string componentName, ref T holder)
                    where T : MonoBehaviour => AddComponent(managersParent, componentName, ref holder);

                AddManagerComponent("actions", ref m_ActionsManager);
                AddManagerComponent("stat_calculator", ref m_StatCalculator);
                AddManagerComponent("expiration", ref m_ExpirationManager);
                AddManagerComponent("cooldowns", ref m_CooldownsController);
                AddManagerComponent("channeling", ref m_ChannelingManager);

                if (Inventory) AddManagerComponent("inventory", ref m_InventoryManager);

                if (Equipment) AddManagerComponent("equipment", ref m_EquipmentManager);

                if (Inventory || Equipment)
                    AddManagerComponent("item_manager", ref m_ItemManager);

                if (Buffs) AddManagerComponent("buffs", ref m_BuffsManager);

                if (Stats) AddManagerComponent("stats", ref m_StatsManager);

                if (Combat) AddManagerComponent("combat", ref m_CombatManager);

                if (Conditions)
                    AddManagerComponent("status_effects", ref m_StatusEffectsManager);

                if (Overheads) AddManagerComponent("overheads", ref m_OverheadsManager);

                if (Skills) AddManagerComponent("skills", ref m_SkillsManager);
            }

            
            public void WithHealthUI()
            {
                if (!Health) return;

                if (m_HealthInitialized) return;

                m_HealthInitialized = true;
                
                var model = GetComponentInChildren<CharacterModelController>();

                var cmp = new GameObject("health_display");
                    
                cmp.transform.SetParent(model.HeadLocation.transform, false);

                cmp.transform.localPosition = new Vector3(0, 1.14f, 0);
                    
                var u = cmp.AddComponent<HealthUIController>();
                
                u.Initialize(m_HealthController);
            }

            private void PrepareCharacter()
            {
                if (Health)
                {
                    m_HealthController.Initialize(CurrentHealth, MaxHealth);

                    if (HealthOverhead)
                    {
                        WithHealthUI();
                    }
                }

                if (Stats && BaseStats)
                {
                    m_StatsController.Initialize(BaseStats.Stats);
                }

                if (Inventory)
                {
                    m_InventoryController.Initialize(InventorySize);

                    if (StartingInventory != null && StartingInventory.Length > 0)
                    {
                        foreach (var itemSo in StartingInventory)
                        {
                            if (!itemSo) continue;

                            var itemInstance = itemSo.MakeInstanceWithCount();

                            m_InventoryController.HandleRequest(
                                ChangeRequestFactory.Add(itemInstance.StorageData));
                        }
                    }
                }

                if (Buffs && StartingBuffs != null && StartingBuffs.Length > 0)
                {
                    foreach (var startingBuff in StartingBuffs)
                    {
                        if (!startingBuff) continue;

                        m_BuffController.AttemptAdd(new(startingBuff.Metadata, gameObject, 1));
                    }
                }

                if (Equipment)
                {
                    m_EquipmentController.Initialize(EquipmentSlots);

                    if (StartingEquipment != null && StartingEquipment.Length > 0)
                    {
                        foreach (var equipmentItem in StartingEquipment)
                        {
                            if (equipmentItem == null || equipmentItem.Item == null) continue;

                            var instance = equipmentItem.Item.MakeInstanceWithCount();

                            var request = new EquipRequest()
                                {Item = instance, Slot = equipmentItem.Slot};

                            m_EquipmentController.HandleEquipRequest(request);
                        }
                    }
                }

                if (Skills)
                {
                    m_SkillsController.Initialize(StartingSkills.Select(x => x.GetMetadata));
                }
            }

            private void AddComponent<T>(Transform parent, string componentName, ref T holder)
                where T : MonoBehaviour
            {
                var i = new GameObject(componentName);

                i.transform.parent = parent;

                holder = i.AddComponent<T>();
            }

            protected virtual void ConfigureTargetingSystem(TargetProvider provider)
            {
            }

            protected virtual void SetupGameHooks()
            {
            }
        }
    }
}